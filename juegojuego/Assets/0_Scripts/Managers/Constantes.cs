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
                new Vector3(0.872f,0.486f,-1.185f),
                new Vector3(-0.028f,0.423f,-0.994f),
                new Vector3(-0.832f,0.422f,-1.092f),
                new Vector3(0.453f,1.237f,-1.694f),
                new Vector3(-0.486f,1.232f,-1.694f)
            };
            public const float LONGITUD_COCHE = 6.0f;
            public const float ESCALA_REDUCCION = 5.0f;
            public const string TAG_CAMARA = "CamaraInventario";
            public static readonly Color COLOR_ILUMINADO = new Color(0.2f, 0.2f, 0.2f, 1f);
        }

        /*
         * Variables relacionadas con la UI
         */
        public static class UI
        {
            public const string TAG_ICONO_INVENTARIO = "IconoInventario";
            public static readonly Vector3 POSICION_BOTON_MOSTRAR = new Vector3(-400.0f, -260.0f, 0.0f);
            public static readonly Vector3 POSICION_BOTON_OCULTAR = new Vector3(-400.0f, -345.0f, 0.0f);
            public const float CANTIDAD_MOVIMIENTO_BOTON = 1.0f;

            public const string TAG_NOTIFICACION_INVENTARIO = "NotificacionInventario";
            public static readonly Vector3 POSICION_NOTIFICACION_MOSTRAR = new Vector3(-190.0f, -258.0f, 0.0f);
            public static readonly Vector3 POSICION_NOTIFICACION_OCULTAR = new Vector3(-190.0f, -340.0f, 0.0f);
            public const float CANTIDAD_MOVIMIENTO_NOTIFICACION = 4.0f;
            public const float CANTIDAD_TIEMPO_NOTIFICACION = 1.0f;
            public const string TEXTO_NOTIFICACION_OBJETO_RECOGIDO = "Objeto recogido";
            public const string TEXTO_NOTIFICACION_OBJETO_SOLTADO = "Objeto soltado";
            public const string TEXTO_NOTIFICACION_OBJ_FOCUSEADO = "- Soltar objeto";
            public const string TEXTO_NOTIFICACION_INV_LLENO = "Inventario lleno";
            public const string TEXTO_NOTIFICACION_OBJETO_RECOGIDO_EN = "Item picked up";
            public const string TEXTO_NOTIFICACION_OBJETO_SOLTADO_EN = "Item dropped";
            public const string TEXTO_NOTIFICACION_OBJ_FOCUSEADO_EN = "- Drop item";
            public const string TEXTO_NOTIFICACION_INV_LLENO_EN = "Inventory full";
        }
    }
    
    /*
     * Variables relacionadas con el jugador
     */
    public static class Player
    {
        public const string TAG_PLAYER = "Player";
        public const string NOMBRE_GO = "CarPlayer";

        public const string TAG_REAR_WHEELS = "RearWheel";
    }

    /*
     * Variables relacionadas con las camaras
     */
    public static class Camaras
    {
        public const string TAG_CAMARA_EXTERIORES = "CamaraExteriores";
        public const string TAG_CAMARA_INTERIORES = "CamaraInteriores";
        public const string TAG_CAMARA_CORTES = "CamaraCortes";
        public const string TAG_CAMARA_INVENTARIO = "CamaraInventario";

        public const string ANIMACION_FADE_TO_BLACK = "FadeToBlack";
        public const string ANIMACION_FADE_TO_WHITE = "FadeToWhite";
    }

    /*
     * Variables relacionadas con el juego
     */
    public static class Juego
    {
        public const string TAG_RESPAWN = "Respawn";
    }

    public static class Objetos
    {
        public const string TAG_OBJETO_RECOGIBLE = "Recogible";
        public const string TAG_PUERTA_LEVADIZA = "PuertaLevadiza";
    }

    public static class DebugUI
    {
        public const string TAG_DEBUG_UI = "Debug";
    }
}
