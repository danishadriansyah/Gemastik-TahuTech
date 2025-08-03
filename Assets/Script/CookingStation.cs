using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Mengelola logika untuk sebuah stasiun memasak (Wok, Panci, dll).
/// </summary>
public class CookingStation : MonoBehaviour
{
    [Header("Pengaturan Stasiun")]
    // Daftar semua resep yang BISA dimasak di stasiun ini.
    public List<RecipeData> availableRecipes;

    [Header("Sprite State")]
    // Hubungkan sprite untuk setiap keadaan.
    public Sprite idleSprite;
    public Sprite cookingSprite;
    public Sprite cookedSprite;

    // Komponen SpriteRenderer dari stasiun ini.
    private SpriteRenderer spriteRenderer;
    // Menyimpan bahan yang sudah dimasukkan oleh player.
    private List<ItemData> currentIngredients = new List<ItemData>();
    // Status stasiun saat ini.
    private bool isCooking = false;
    private bool isCooked = false;

    void Start()
    {
        // Mengambil komponen SpriteRenderer saat permainan dimulai.
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = idleSprite; // Pastikan sprite awal adalah idle.
    }

    /// <summary>
    /// Method ini dipanggil oleh player saat berinteraksi.
    /// </summary>
    public void Interact()
    {
        // Jika sudah matang, player bisa mengambil hasilnya.
        if (isCooked)
        {
            TakeResult();
            return;
        }

        // Jika sedang memasak, jangan lakukan apa-apa.
        if (isCooking)
        {
            Debug.Log("Sedang memasak...");
            return;
        }

        // Jika tidak, buka UI memasak.
        // (Kita akan buat CookingUIManager di langkah berikutnya)
        CookingUIManager.instance.OpenCookingUI(this);
    }

    /// <summary>
    /// Menambahkan bahan ke dalam stasiun. Dipanggil dari UI.
    /// </summary>
    public void AddIngredient(ItemData ingredient)
    {
        if (!isCooking && !isCooked)
        {
            currentIngredients.Add(ingredient);
            Debug.Log("Menambahkan " + ingredient.itemName);
            // Di sini kamu bisa menambahkan efek visual/suara saat bahan masuk.
        }
    }

    /// <summary>
    /// Memulai proses memasak. Dipanggil dari UI.
    /// </summary>
    public void StartCooking()
    {
        RecipeData recipeToCook = FindMatchingRecipe();
        if (recipeToCook != null)
        {
            StartCoroutine(Cook(recipeToCook));
        }
        else
        {
            Debug.Log("Bahan tidak cocok dengan resep manapun!");
            // Kamu bisa menambahkan feedback ke player di sini.
            // Dan kembalikan bahan ke inventory player.
            ClearIngredients();
        }
    }

    // Coroutine untuk proses memasak dengan timer.
    private IEnumerator Cook(RecipeData recipe)
    {
        isCooking = true;
        spriteRenderer.sprite = cookingSprite; // Ganti sprite menjadi "memasak".
        Debug.Log("Memasak " + recipe.cookedDish.itemName);

        // Hapus bahan dari inventory player.
        foreach (var ingredient in recipe.ingredients)
        {
            InventoryManager.instance.RemoveItem(ingredient);
        }

        // Tunggu sesuai waktu memasak.
        yield return new WaitForSeconds(recipe.cookingTime);

        isCooking = false;
        isCooked = true;
        spriteRenderer.sprite = cookedSprite; // Ganti sprite menjadi "matang".
        Debug.Log(recipe.cookedDish.itemName + " sudah matang!");
    }

    // Mengambil hasil masakan.
    private void TakeResult()
    {
        RecipeData cookedRecipe = FindMatchingRecipe();
        if (cookedRecipe != null)
        {
            // Tambahkan masakan jadi ke inventory player.
            InventoryManager.instance.AddItem(cookedRecipe.cookedDish);
            Debug.Log("Mengambil " + cookedRecipe.cookedDish.itemName);
        }

        // Reset stasiun ke keadaan semula.
        ResetStation();
    }

    // Mencari resep yang cocok dengan bahan saat ini.
    private RecipeData FindMatchingRecipe()
    {
        foreach (var recipe in availableRecipes)
        {
            if (recipe.ingredients.Count != currentIngredients.Count)
                continue;

            bool allMatch = true;
            List<ItemData> tempIngredients = new List<ItemData>(currentIngredients);
            foreach (var requiredIngredient in recipe.ingredients)
            {
                if (tempIngredients.Contains(requiredIngredient))
                {
                    tempIngredients.Remove(requiredIngredient);
                }
                else
                {
                    allMatch = false;
                    break;
                }
            }

            if (allMatch) return recipe;
        }
        return null;
    }

    // Membersihkan bahan dan mereset stasiun.
    public void ResetStation()
    {
        ClearIngredients();
        isCooked = false;
        isCooking = false;
        spriteRenderer.sprite = idleSprite;
    }

    public void ClearIngredients()
    {
        currentIngredients.Clear();
    }
}
