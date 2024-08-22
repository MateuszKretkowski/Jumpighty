using UnityEngine;
using UnityEditor;

public class MaterialUpdater : EditorWindow
{
    [MenuItem("Tools/Update Materials")]
    public static void ShowWindow()
    {
        GetWindow<MaterialUpdater>("Update Materials");
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Update All Materials"))
        {
            UpdateMaterials();
        }
    }

    private static void UpdateMaterials()
    {
        // Get all model assets in the project
        string[] guids = AssetDatabase.FindAssets("t:Model", new[] { "Assets/Nature-Asset-Pack/Models" });

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject model = AssetDatabase.LoadAssetAtPath<GameObject>(path);

            if (model != null)
            {
                Renderer[] renderers = model.GetComponentsInChildren<Renderer>();

                foreach (Renderer renderer in renderers)
                {
                    Material[] materials = renderer.sharedMaterials;

                    for (int i = 0; i < materials.Length; i++)
                    {
                        Material oldMaterial = materials[i];
                        if (oldMaterial != null)
                        {
                            // Create the new material name by adding "1" at the end
                            string newMaterialName = oldMaterial.name + " 1";
                            string[] materialGuids = AssetDatabase.FindAssets(newMaterialName + " t:Material");

                            if (materialGuids.Length > 0)
                            {
                                string newMaterialPath = AssetDatabase.GUIDToAssetPath(materialGuids[0]);
                                Material newMaterial = AssetDatabase.LoadAssetAtPath<Material>(newMaterialPath);

                                if (newMaterial != null)
                                {
                                    materials[i] = newMaterial;
                                }
                                else
                                {
                                    Debug.LogWarning($"Material {newMaterialName} not found for {model.name}");
                                }
                            }
                        }
                    }

                    renderer.sharedMaterials = materials;
                }

                EditorUtility.SetDirty(model);
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("Materials updated successfully.");
    }
}
