let MAP; // Variable global para el mapa
function initMap() {
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
    console.log('esta corriendo');
    // Llama a la función para cargar la API de Google Maps
    loadGoogleMapsAPI();
})

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
                this.actividades = datos;
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
            let marker = new google.maps.marker.AdvancedMarkerElement({
                position: { lat: parseFloat(actividad.lat), lng: parseFloat(actividad.lon) },
                map: this.mapa,
                title: actividad.mensajeDelAnfitrion
            });

            // Agregar evento de clic al marcador
            marker.addListener('gmp-click', function () {
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



///////////////////////////
// Implementación mínima
// del Patrón Observador con Herencia
///////////////////////////

//// Observador base: garantiza método actualizar
//class Observador {
//  constructor(nombre) {
//    this.nombre = nombre;
//  }

//  actualizar(datos) {
//    console.log(`${this.nombre}:`, datos);
//  }
//}

//// Ejemplo de herencia: un observador especializado
//class ObservadorConcreto extends Observador {
//  constructor(nombre, prefijo) {
//    super(nombre);
//    this.prefijo = prefijo;
//  }

//  actualizar(datos) {
//    // Lógica propia antes o después de llamar al padre
//    console.log(`${this.prefijo} ${this.nombre} recibió:`, datos);
//    // Opcional: llamar al método del padre
//    super.actualizar(datos);
//  }
//}

//// Sujeto (Subject)
//class Sujeto {
//  constructor() {
//    this.observadores = [];
//  }

//  suscribir(observador) {
//    if (!(observador instanceof Observador)) {
//      throw new Error('El observador debe ser una instancia de Observador');
//    }
//    this.observadores.push(observador);
//  }

//  desuscribir(observador) {
//    this.observadores = this.observadores.filter(o => o !== observador);
//  }

//  notificar(datos) {
//    this.observadores.forEach(o => o.actualizar(datos));
//  }
//}

//// Uso mínimo:
//const sujeto = new Sujeto();

//// Instancias de Observador base y ObservadorConcreto
//const observadorA = new Observador('Observador A');
//const observadorB = new ObservadorConcreto('Observador B', '[Prefijo]');

//// Registrar observadores
//sujeto.suscribir(observadorA);
//sujeto.suscribir(observadorB);

//// Notificar a todos
//sujeto.notificar('¡Evento con herencia!');

//// Desregistrar uno
//sujeto.desuscribir(observadorA);

//// Volver a notificar
//sujeto.notificar('¡Segundo evento!');