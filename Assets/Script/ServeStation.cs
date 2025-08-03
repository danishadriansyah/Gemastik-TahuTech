using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ditempatkan pada area penyajian (kotak hijau).
/// </summary>
public class ServeStation : MonoBehaviour
{
    /// <summary>
    /// Dipanggil saat player berinteraksi dengan area ini.
    /// </summary>
    public void Serve(GameObject servedPlate)
    {
        Debug.Log("Masakan berhasil disajikan!");

        // Dapatkan komponen PlateStation dari piring yang disajikan.
        PlateStation plate = servedPlate.GetComponent<PlateStation>();
        if (plate != null)
        {
            // Mulai proses respawn untuk piring tersebut.
            plate.StartRespawn();
        }

        // Sembunyikan piring yang sudah disajikan.
        servedPlate.SetActive(false);

        // Di sini kamu bisa menambahkan logika skor, uang, atau kepuasan pelanggan.
    }
}
