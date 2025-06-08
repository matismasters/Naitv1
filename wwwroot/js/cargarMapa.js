let MAP; // Variable global para el mapa
function initMap() {
    var customStyles = [
        {
            featureType: "poi",
            elementType: "all",
            stylers: [
                { visibility: "off" }
            ]
        },
        {
            featureType: "transit",
            elementType: "all",
            stylers: [
                { visibility: "off" }
            ]
        },
        {
            featureType: "road.highway",
            elementType: "labels.icon",
            stylers: [
                { visibility: "off" }
            ]
        }
    ];

    MAP = new google.maps.Map(document.getElementById('map'), {
        center: { lat: -34.471388888889, lng: -57.844166666667 },
        zoom: 14,
        disableDefaultUI: true,
        /*styles: customStyles,*/
        /*mapId: 'mainMap'*/
    });

    const registroParticipacionController = new RegistroParticipacionController();
    registroParticipacionController.registrarParticipacionTodoElTiempo();

    // Iniciamos observación
    const actividadesVisibles = new Observado();
    const obsContador = new ObservadorContador('contadorActividades');
    const obsMapa = new ObservadorMapa(MAP);

    actividadesVisibles.agregarObservador(obsContador);
    actividadesVisibles.agregarObservador(obsMapa);
    actividadesVisibles.traerActividades();
    actividadesVisibles.traerActividadesTodoElTiempo();

    // Comprobar si el navegador soporta Geolocalización
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(
            function (position) {
                // Obtenemos la posición actual del usuario
                var pos = {
                    lat: position.coords.latitude,
                    lng: position.coords.longitude
                };

                // Centra el mapa en la ubicación actual
                MAP.setCenter(pos);

                // Coloca un marcador (pin) en la ubicación actual
                new google.maps.marker.AdvancedMarkerElement({
                    position: pos,
                    map: MAP,
                    title: 'Aqui estoy'
                });
            },
            function () {
                handleLocationError(true, MAP.getCenter());
            }
        );
    } else {
        // El navegador no soporta Geolocalización
        handleLocationError(false, MAP.getCenter());
    }
}

// Función para cargar la API de Google Maps de forma programática
function loadGoogleMapsAPI() {
    var apiKey = GOOGLE_MAPS_API_KEY; // Reemplaza con tu clave de API
    var script = document.createElement('script');
    // Se construye la URL incluyendo la clave y el callback que llama a initMap
    script.src = "https://maps.googleapis.com/maps/api/js?key=" + apiKey + "&callback=initMap&libraries=marker";
    script.async = true;
    script.defer = true;
    document.head.appendChild(script);
}

document.addEventListener('DOMContentLoaded', function () {
    // Llama a la función para cargar la API de Google Maps
    loadGoogleMapsAPI();
})

class RegistroParticipacionController {
    registrarParticipacionTodoElTiempo() {
        this.registrarParticipacion(); // Llamada inicial
        setInterval(() => {
            this.registrarParticipacion();
        }, 3000); // 20000 ms = 20 segundos
    }

    registrarParticipacion() {
        console.log('registrarParticipacion() llamado');


        // Comprobar si el navegador soporta Geolocalización
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(function (position) {
                // Obtenemos la posición actual del usuario
                var pos = {
                    lat: position.coords.latitude,
                    lon: position.coords.longitude
                };

                fetch(`/RegistroActividades/Participar?latUsuario=${pos.lat}&lonUsuario=${pos.lon}`, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    }
                })
                    .then(response => response.json())
                    .then((datos) => {
                        console.log('Participación registrada:', datos);
                    })
                    .catch(error => console.error('Error al registrar la participacion:', error));

                console.log(pos);
            });
        }
    }
}

class Observado {
    constructor() {
        this.observadores = [];
        this.actividades = [];
    }

    notificarObservadores(actividades) {
        this.observadores.forEach(
            (observador) => observador.recargarActividades(actividades)
        );
    }

    agregarObservador(observador) {
        this.observadores.push(observador)
    }

    traerActividades() {
        fetch('/Actividades/Visibles')
            .then(response => response.json())
            .then((datos) => {
                console.log('Actividades recibidas:', datos);
                this.actividades = datos;
                //console.log(datos);
                this.notificarObservadores(datos);
            })
            .catch(error => console.error('Error al cargar las actividades:', error));
    }

    traerActividadesTodoElTiempo() {
        setInterval(() => {
            this.traerActividades();
        }, 5000); // 5000 ms = 5 segundos
    }
}

class ObservadorContador {
    constructor(idDivContador) {
        this.idDivContador = idDivContador;
    }

    recargarActividades(actividades) {
        // Aca actualizamos el contador
        const divContador = document.getElementById(this.idDivContador);
        divContador.innerHTML = actividades.length;
    }
}

class ObservadorMapa {
    constructor(mapa) {
        this.mapa = mapa;
        this.markers = [];
    }

    recargarActividades(actividades) {
        this.borrarMarcadores();
        // Actividades
        actividades.forEach((actividad) => {
            // 2) Crea un <img> y ajusta sus atributos (src, tamaño, posición CSS, etc.)
            const imgIcon = document.createElement("img");
            imgIcon.src = actividad.urlImagenMarcador;
            imgIcon.style.width = "100px";   // Ancho en px
            imgIcon.style.height = "100px";  // Alto en px

            // Para que el “pico” quede centrado en la base de la imagen, puedes
            // usar margin o position absoluta; pero AdvancedMarker te ancla el elemento
            // completo en el centro de su caja, así que si la punta está abajo, puedes
            // desplazarlo con CSS. Por ejemplo:
            imgIcon.style.transform = "translate(-50%, -100%)";
            imgIcon.style.position = "absolute"; // para que el transform funcione bien

            let marker = new google.maps.marker.AdvancedMarkerElement({
                position: { lat: parseFloat(actividad.lat), lng: parseFloat(actividad.lon) },
                map: this.mapa,
                content: imgIcon,
                title: actividad.mensajeDelAnfitrion
            });

            // Agregar evento de clic al marcador
            marker.addListener('click', () => {
                // Configurar el contenido del modal dinámicamente
                document.getElementById('modalTitle').innerText = actividad.tipoActividad;
                document.getElementById('modalBody').innerText = actividad.mensajeDelAnfitrion;

                // Mostrar el modal de Bootstrap
                let modal = new bootstrap.Modal(document.getElementById('actividadModal'));
                modal.show();
            });

            this.markers.push(marker);
        });
    }

    borrarMarcadores() {
        this.markers.forEach((marker) => marker.setMap(null));
        this.markers = [];
    }
}
