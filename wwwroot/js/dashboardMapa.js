let MAPA_ADMIN;
let marcadores = [];

window.refresco = true;  // me indica por defecto que sí tiene que haber refresh automático del mapa.
window.intervaloMapa = null; // va a guardar el ID del setInterval para poder frenarlo después.

function initMap() {    

    MAPA_ADMIN = new google.maps.Map(document.getElementById('map'), {
        center: { lat: -34.90, lng: -56.18 }, 
        zoom: 12,
        disableDefaultUI: true,
        gestureHandling: 'greedy',
    });

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
                MAPA_ADMIN.setCenter(pos);

                // Coloca un marcador (pin) en la ubicación actual
                new google.maps.Marker({
                    position: pos,
                    map: MAPA_ADMIN,
                    title: 'Aqui estoy'
                });
            },
            function () {
                handleLocationError(true, MAPA_ADMIN.getCenter());
            }
        );
    } else {
        // El navegador no soporta Geolocalización
        handleLocationError(false, MAPA_ADMIN.getCenter());
    }
    
    
    traerYActualizarActividades(); // carga por primera vez

    if (window.refresco) { // al inicio refresco es true, entonces entra al refresh
        window.intervaloMapa = setInterval(() => { // intervaloMapa va a guardar el ID del setInterval para poder frenarlo después.
            traerYActualizarActividades();
        }, 10000);
    }
    
    //setInterval(traerYActualizarActividades, 10000); // hace refresh cada 10 seg
    
    
}  

/*async function traerYActualizarActividades(filtro = null) {
    try {
        const response = await fetch('/Actividades/Visibles');
        const actividades = await response.json();
        //console.log(actividades);

        actualizarMarcadores(actividades);

    } catch (error) {
        console.error('Error al traer actividades:', error);
    }   

}*/

async function traerYActualizarActividades(filtro = null) {
    try {

        //const response = await fetch('/Actividades/Visibles');
        const response = await fetch('/PanelAdmin/ActividadesFiltradas', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(filtro)
        });

        const actividades = await response.json();
        console.log("DEsde funcion para mostra en mapa===========");
        console.log(actividades);

        actualizarMarcadores(actividades);

    } catch (error) {
        console.error('Error al traer actividades:', error);
    }   

}

function actualizarMarcadores(actividades) {
    // Borra marcadores anteriores
    marcadores.forEach(marker => marker.setMap(null));
    marcadores = [];

    actividades.forEach(actividad => {

        const marcador = new google.maps.Marker({
            position: { lat: parseFloat(actividad.lat), lng: parseFloat(actividad.lon) },
            map: MAPA_ADMIN,
            title: actividad.mensajeDelAnfitrion
        });

        // Agregar evento de clic al marcador
        marcador.addListener('click', () => {
            // Configurar el contenido del modal dinámicamente
            document.getElementById('modalTitle').innerText = actividad.tipoActividad;
            document.getElementById('modalBody').innerText = actividad.mensajeDelAnfitrion;

            // Mostrar el modal de Bootstrap
            let modal = new bootstrap.Modal(document.getElementById('actividadModal'));
            modal.show();
        });

        marcadores.push(marcador);
    });
}

function loadGoogleMapsAPI() {
    var apiKey = GOOGLE_MAPS_API_KEY; 
    var script = document.createElement('script');

    // Se construye la URL incluyendo la clave y el callback que llama a initMap
    script.src = "https://maps.googleapis.com/maps/api/js?key=" + apiKey + "&callback=initMap&libraries=marker";
    script.async = true;
    script.defer = true;
    document.head.appendChild(script);
}

document.addEventListener('DOMContentLoaded', function () {
    //console.log('esta corriendo');
    
    loadGoogleMapsAPI();
})



