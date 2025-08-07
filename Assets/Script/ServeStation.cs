using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServeStation : MonoBehaviour
{
    public void Serve(GameObject servedPlate)
    {
        PlateStation plateScript = servedPlate.GetComponent<PlateStation>();
        if (plateScript == null) return;

        // Dapatkan data masakan dari piring.
        ItemData servedDish = plateScript.GetHeldDish();

        // Jika ada masakan di piring...
        if (servedDish != null)
        {
            // LAPORKAN ke OrderManager untuk dicek.
            OrderManager.instance.CheckServedDish(servedDish);
        }

        // Sembunyikan piring dan mulai proses respawn.
        servedPlate.SetActive(false);
        StartCoroutine(RespawnPlateCoroutine(servedPlate, 5f)); // Anda bisa ubah waktunya
    }

    private IEnumerator RespawnPlateCoroutine(GameObject plateToRespawn, float delay)
    {
        yield return new WaitForSeconds(delay);
        PlateStation plateScript = plateToRespawn.GetComponent<PlateStation>();
        if (plateScript != null)
        {
            plateScript.ResetAndReactivate();
        }
    }
}
