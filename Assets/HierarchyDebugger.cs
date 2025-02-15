using System;
using UnityEngine;
using System.Text;

public class HierarchyLogger : MonoBehaviour
{
    private int interactionsCount = 0; // Counter for "Interactions" components
    
    private float timer = 0f;
    void Start()
    {
        LogHierarchy();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 2f)
        {
            LogHierarchy();
            timer = 0f;
        }
        
    }

    void LogHierarchy()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("=== GAMEOBJECT HIERARCHY ===");

        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            // Get parent name (or None if it's a root object)
            string parentName = obj.transform.parent ? obj.transform.parent.name : "None";

            // Print object and parent
            sb.AppendLine($"HIERARCHY Object: {obj.name} | Parent: {parentName}");

            // Check if the object has an "Interactions" component
            Interactions interactionsComponent = obj.GetComponent<Interactions>();
            if (interactionsComponent != null)
            {
                interactionsCount++;
                sb.AppendLine($"⚠️ HIERARCHY Found 'Interactions' component! Parent: {parentName}");
            }

            // Get and log all components attached to this GameObject
            Component[] components = obj.GetComponents<Component>();
            sb.Append("HIERARCHY  Components: ");
            foreach (Component comp in components)
            {
                sb.Append(comp.GetType().Name + ", ");
            }
            sb.Length -= 2; // Remove last comma
            sb.AppendLine();
        }

        sb.AppendLine($"HIERARCHY === Total 'Interactions' Components Found: {interactionsCount} ===");

        // Print everything to log
        Debug.Log(sb.ToString());
    }
}