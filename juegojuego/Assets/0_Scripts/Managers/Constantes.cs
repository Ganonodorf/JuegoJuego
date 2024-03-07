using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class Constantes
{
    /*
     * Variables relacionadas con el inventario
     */

    public const float CAPACIDAD_INVENTARIO = 5.0f;
    public static readonly List<UnityEngine.Vector3> POSICIONES_INVENTARIO = new List<UnityEngine.Vector3>
    {
        new UnityEngine.Vector3(1.3f, 3.7f, -0.8f),
        new UnityEngine.Vector3(0.0f, 3.7f, -0.8f),
        new UnityEngine.Vector3(-1.3f, 3.7f, -0.8f),
        new UnityEngine.Vector3(-0.6f, 4.5f, -1.3f),
        new UnityEngine.Vector3(0.6f, 4.5f, -1.3f)
    };

    public const float ESCALA_REDUCCION = 2.0f;
    public const string NOMBRE_CAMARA_INV_GO = "CamaraInventario";


    /*
     * Variables relacionadas con el jugador
     */

    public const string NOMBRE_PLAYER_GO = "CarPlayer";
}
