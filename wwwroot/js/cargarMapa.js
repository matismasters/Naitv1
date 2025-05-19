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

    // Inicialización del mapa, centrado en Madrid como ejemplo
    MAP = new google.maps.Map(document.getElementById('map'), {
        center: {lat: -34.471388888889, lng: -57.844166666667},
        zoom: 14,
        disableDefaultUI: true,
        styles: customStyles
    });

    traerActividadesTodoElTiempo();

    // Comprobar si el navegador soporta Geolocalización
    if (navigator.geolocation) {
      navigator.geolocation.getCurrentPosition(
        function(position) {
          // Obtenemos la posición actual del usuario
          var pos = {
            lat: position.coords.latitude,
            lng: position.coords.longitude
          };

          // Centra el mapa en la ubicación actual
          MAP.setCenter(pos);

          // Coloca un marcador (pin) en la ubicación actual
          new google.maps.Marker({
            position: pos,
            map: MAP
          });
        },
        function() {
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
    script.src = "https://maps.googleapis.com/maps/api/js?key=" + apiKey + "&callback=initMap";
    script.async = true;
    script.defer = true;
    document.head.appendChild(script);
}

document.addEventListener('DOMContentLoaded', function () {
    console.log('esta corriendo');
    // Llama a la función para cargar la API de Google Maps
    loadGoogleMapsAPI();
})

function traerActividadesTodoElTiempo() {
    setInterval(function () {
        // Llama a la función para cargar las actividades cada 5 segundos
        console.log("traemos actividades");
        traerActividades();
    }, 5000); // 5000 ms = 5 segundos
}
function traerActividades() {
    fetch('/Actividades/Visibles')
        .then(response => response.json())
        .then(recargarActividades)
        .catch(error => console.error('Error al cargar las actividades:', error));
}
function recargarActividades(actividades) {
    // Aca actualizamos el contador
    const divContador = document.getElementById('contadorActividades');
    divContador.innerHTML = actividades.length;

    // Actividades
    actividades.forEach(function (actividad) {
        console.log(actividad);

        let marker = new google.maps.Marker({
            position: { lat: parseFloat(actividad.lat), lng: parseFloat(actividad.lon) },
            map: MAP,
            title: actividad.mensajeDelAnfitrion
        });

        // Agregar evento de clic al marcador
        marker.addListener('click', function () {
            // Configurar el contenido del modal dinámicamente
            document.getElementById('modalTitle').innerText = actividad.tipoActividad;
            document.getElementById('modalBody').innerText = actividad.mensajeDelAnfitrion;

            // Mostrar el modal de Bootstrap
            let modal = new bootstrap.Modal(document.getElementById('actividadModal'));
            modal.show();
        });
    });
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


