using UnityEditor;
using UnityEngine;

[CustomEditor (typeof (ItemToBuy))]
public class EditorItemToBuy : Editor {
    //ItemType type;
    ItemToBuy itemToBuy;
    private SerializedProperty priceText = null;

    //Weapon
    private SerializedProperty weapon = null;
    private SerializedProperty buyButtonText = null;
    private SerializedProperty price = null;

    //Health
    private SerializedProperty healSoundIndex = null;

    private void OnEnable () {
        itemToBuy = (ItemToBuy) target;

        //Weapon
        weapon = serializedObject.FindProperty ("weapon");
        buyButtonText = serializedObject.FindProperty ("buyButtonText");
        price = serializedObject.FindProperty ("price");
        priceText = serializedObject.FindProperty ("priceText");

        //Hp
        healSoundIndex = serializedObject.FindProperty ("healSoundIndex");
    }

    public override void OnInspectorGUI () // Переопределяем метод который рисует испектор
    {
        EditorGUI.BeginChangeCheck ();

        EditorGUILayout.BeginVertical (); // Это чтобы элементы были вертикально друг за другом

        EditorGUILayout.PropertyField (price);
        if (itemToBuy.priceText == null) EditorGUILayout.PropertyField (priceText);

        itemToBuy.type = (ItemType) EditorGUILayout.EnumPopup ("Item Type", itemToBuy.type);

        if (itemToBuy.type == ItemType.weapon) {
            EditorGUILayout.PropertyField (weapon);
            if (itemToBuy.buyButtonText == null) EditorGUILayout.PropertyField (buyButtonText);
        } else if (itemToBuy.type == ItemType.health) {
            itemToBuy.restoreHp = EditorGUILayout.IntField ("RestoreHP", itemToBuy.restoreHp);
            EditorGUILayout.PropertyField (healSoundIndex);
        }

        EditorGUILayout.EndVertical ();

        if (EditorGUI.EndChangeCheck ()) {
            //Применение изменений
            serializedObject.ApplyModifiedProperties ();
        }
    }
}