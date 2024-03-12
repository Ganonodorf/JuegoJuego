using System.Collections.Generic;
using UnityEngine;

// Cuando haces esto, le dices que siempre que se llame a Vector3, va a ser el de UnityEngine, y no otro.
// Esto se ha puesto porque ha dado conflicto
using Vector3 = UnityEngine.Vector3;

namespace Constantes
{
    /*
     * Variables relacionadas con el inventario
     */
    public static class Inventario
    {
        /*
         * Variables relacionadas con el Manager
         */
        public static class Manager
        {
            public const float CAPACIDAD = 5.0f;
            public static readonly List<Vector3> POSICIONES = new List<Vector3>
            {
                new Vector3(-1.3f, 3.7f, -0.8f),
                new Vector3(0.0f, 3.7f, -0.8f),
                new Vector3(1.3f, 3.7f, -0.8f),
                new Vector3(-0.6f, 4.5f, -1.3f),
                new Vector3(0.6f, 4.5f, -1.3f)
            };
            public const float LONGITUD_COCHE = 6.0f;
            public const float ESCALA_REDUCCION = 2.0f;
            public const string NOMBRE_CAMARA_GO = "CamaraInventario";
            public static readonly Color COLOR_ILUMINADO = new Color(0.98f, 1.0f, 0.78f, 0.01f);
        }

        /*
         * Variables relacionadas con la UI
         */
        public static class UI
        {
            public const string NOMBRE_BOTON_GO = "IconoInventario";
            public static readonly Vector3 POSICION_BOTON_MOSTRAR = new Vector3(-460.0f, -260.0f, 0.0f);
            public static readonly Vector3 POSICION_BOTON_OCULTAR = new Vector3(-460.0f, -345.0f, 0.0f);
            public const float CANTIDAD_MOVIMIENTO_BOTON = 1.0f;

            public const string NOMBRE_NOTIFICACION_GO = "PanelNotificacion";
            public static readonly Vector3 POSICION_NOTIFICACION_MOSTRAR = new Vector3(-190.0f, -258.0f, 0.0f);
            public static readonly Vector3 POSICION_NOTIFICACION_OCULTAR = new Vector3(-190.0f, -340.0f, 0.0f);
            public const float CANTIDAD_MOVIMIENTO_NOTIFICACION = 4.0f;
            public const float CANTIDAD_TIEMPO_NOTIFICACION = 1.0f;
            public const string TEXTO_NOTIFICACION_OBJETO_RECOGIDO = "Objeto recogido";
            public const string TEXTO_NOTIFICACION_OBJETO_SOLTADO = "Objeto soltado";
            public const string TEXTO_NOTIFICACION_OBJ_FOCUSEADO = "Pulsa E para soltar el objeto";
            public const string TEXTO_NOTIFICACION_INV_LLENO = "Inventario lleno";
        }
    }
    
    /*
     * Variables relacionadas con el jugador
     */
    public static class Player
    {
        public const string NOMBRE_GO = "CarPlayer";
    }
}
