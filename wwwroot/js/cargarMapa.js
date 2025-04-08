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
    var map = new google.maps.Map(document.getElementById('map'), {
        center: {lat: -34.471388888889, lng: -57.844166666667},
        zoom: 13,
        disableDefaultUI: true,
        styles: customStyles
    });

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
          map.setCenter(pos);

          // Coloca un marcador (pin) en la ubicación actual
          new google.maps.Marker({
            position: pos,
            map: map
          });
        },
        function() {
          handleLocationError(true, map.getCenter());
        }
      );
    } else {
      // El navegador no soporta Geolocalización
      handleLocationError(false, map.getCenter());
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

