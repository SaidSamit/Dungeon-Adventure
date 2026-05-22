using UnityEngine;
using System;

public class UpgradePanelUI : MonoBehaviour
{
    // Variable interna para almacenar temporalmente la orden de la sala activa
    private Action currentRoomCallback;

    public void SetupMenu(Action callback)
    {
        // Guardar en memoria la función de reanudar el juego proveniente de la sala actual
        currentRoomCallback = callback;
    }

    public void ExecuteRoomCallback()
    {
        // Ejecutar la orden almacenada para abrir la puerta y restaurar el tiempo
        currentRoomCallback?.Invoke();
    }
}