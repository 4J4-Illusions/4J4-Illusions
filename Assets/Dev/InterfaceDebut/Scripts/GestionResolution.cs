using UnityEngine;

public class GestionResolution : MonoBehaviour
{
    public ChangementOptionsAvecBoutons optionResolution;

    public void AppliquerResolution()
    {
        switch(optionResolution.Index)
{
    case 0:
        Screen.SetResolution(1920,1080,true);
        break;

    case 1:
        Screen.SetResolution(2560,1440,true);
        break;

    case 2:
        Screen.SetResolution(2880,1620,true);
        break;
    case 3:
        Screen.SetResolution(3200,1800,true);
        break;
    case 4:
        Screen.SetResolution(3840,2160,true);
        break;
    case 5:
        Screen.SetResolution(5120,2880,true);
        break;
    case 6:
        Screen.SetResolution(7680,4320,true);
        break;
    case 7:
        Screen.SetResolution(1280,720,true);
        break;
    case 8:
        Screen.SetResolution(1366,768,true);
        break;
    case 9:
        Screen.SetResolution(1600,900,true);
        break;
}
    }
}