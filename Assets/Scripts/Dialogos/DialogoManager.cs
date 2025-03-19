using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogoManager : Singleton<DialogoManager>
{
    [SerializeField] private GameObject panelDialogo;
    [SerializeField] private Image npcIcono;
    [SerializeField] private TextMeshProUGUI npcNombreTMP;
    [SerializeField] private TextMeshProUGUI npcConversacionTMP;

    // Agregar referencias a los paneles de armas y botones
    [SerializeField] private GameObject panelArmas;
    [SerializeField] private GameObject panelBotones;

    // Referencia al script de movimiento del jugador
    [SerializeField] private PersonajeMovimiento personajeMovimiento;

    public NPCInteraccion NPCDisponible { get; set; }

    private Queue<string> dialogosSecuencia;
    private bool dialogoAnimado;
    private bool despedidaMostrada;

    private void Start()
    {
        dialogosSecuencia = new Queue<string>();
    }

    private void Update()
    {
        if (NPCDisponible == null)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            ConfigurarPanel(NPCDisponible.Dialogo);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (despedidaMostrada)
            {
                AbrirCerrarPanelDialogo(false);
                despedidaMostrada = false;
                return;
            }

            if (dialogoAnimado)
            {
                ContinuarDialogo();
            }
        }
    }

    public void AbrirCerrarPanelDialogo(bool estado)
    {
        panelDialogo.SetActive(estado);

        // Ocultar los paneles de armas y botones mientras el diálogo esté activo
        if (panelArmas != null) panelArmas.SetActive(!estado);
        if (panelBotones != null) panelBotones.SetActive(!estado);
        // Deshabilitar movimiento cuando el diálogo está activo
        if (personajeMovimiento != null)
        {
            personajeMovimiento.enabled = !estado;
        }
    }

    private void ConfigurarPanel(NPCDialogo npcDialogo)
    {
        AbrirCerrarPanelDialogo(true);
        CargarDialogosSencuencia(npcDialogo);

        npcIcono.sprite = npcDialogo.Icono;
        npcNombreTMP.text = $"{npcDialogo.Nombre}:";
        MostrarTextoConAnimacion(npcDialogo.Saludo);
    }

    private void CargarDialogosSencuencia(NPCDialogo npcDialogo)
    {
        if (npcDialogo.Conversacion == null || npcDialogo.Conversacion.Length <= 0)
        {
            return;
        }

        for (int i = 0; i < npcDialogo.Conversacion.Length; i++)
        {
            dialogosSecuencia.Enqueue(npcDialogo.Conversacion[i]);
        }
    }

    private void ContinuarDialogo()
    {
        if (NPCDisponible == null)
        {
            return;
        }

        if (despedidaMostrada)
        {
            return;
        }

        if (dialogosSecuencia.Count == 0)
        {
            string despedida = NPCDisponible.Dialogo.Despedida;
            MostrarTextoConAnimacion(despedida);
            despedidaMostrada = true;
            return;
        }

        string siguienteDialogo = dialogosSecuencia.Dequeue();
        MostrarTextoConAnimacion(siguienteDialogo);
    }

    private IEnumerator AnimarTexto(string oracion)
    {
        dialogoAnimado = false;
        npcConversacionTMP.text = "";
        char[] letras = oracion.ToCharArray();
        for (int i = 0; i < letras.Length; i++)
        {
            npcConversacionTMP.text += letras[i];
            yield return new WaitForSeconds(0.03f);
        }

        dialogoAnimado = true;
    }

    private void MostrarTextoConAnimacion(string oracion)
    {
        StartCoroutine(AnimarTexto(oracion));
    }
}
