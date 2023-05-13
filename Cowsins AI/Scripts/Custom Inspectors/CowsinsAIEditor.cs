using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CowsinsAI))]
public class CowsinsAIEditor : Editor
{
    string[] tabs = { "Variables", "Combat", "Debug" };
    int currentTab = 0;

    override public void OnInspectorGUI()
    {
        serializedObject.Update();
        CowsinsAI cai = target as CowsinsAI;

        EditorGUILayout.BeginVertical();
        currentTab = GUILayout.Toolbar(currentTab, tabs);
        EditorGUILayout.Space(10f);
        EditorGUILayout.EndVertical();

        if (currentTab >= 0 || currentTab < tabs.Length)
        {
            switch (tabs[currentTab])
            {
                case "Variables":
                    EditorGUILayout.LabelField("BASIC VARIABLES", EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("currentState"));
                    EditorGUILayout.Space(5);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("waitTime"));
                    EditorGUILayout.Space(5);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("waypoints"));
                    EditorGUILayout.Space(5);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("searchRadius"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("searchAngle"));
                    EditorGUILayout.Space(5);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("player"));
                    EditorGUILayout.Space(5);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("targetMask"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("obstructionMask"));
                    EditorGUILayout.Space(5);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("canSeePlayer"));
                    EditorGUILayout.Space(5);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("waitTimeToSearch"));
                    break;
                case "Combat":
                    EditorGUILayout.LabelField("COMBAT OPTIONS", EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("shooter"));
                    if (cai.shooter == true)
                    {
                        EditorGUILayout.LabelField("SHOOTER OPTIONS", EditorStyles.boldLabel);
                        EditorGUI.indentLevel++;
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("projectile"));
                        EditorGUILayout.Space(5);
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("firePoint"));
                        EditorGUILayout.Space(5);
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("shooterAnimator"));
                        EditorGUILayout.Space(5);
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("timeBetweenAttacks"));
                        EditorGUILayout.Space(5);
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("shootDistance"));
                        EditorGUILayout.Space(5);
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("inShootingDistance"));
                        EditorGUI.indentLevel--;
                    }
                    EditorGUILayout.Space(5);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("melee"));
                    EditorGUILayout.Space(5);
                    if (cai.melee == true)
                    {
                        EditorGUILayout.LabelField("MELEE OPTIONS", EditorStyles.boldLabel);
                        EditorGUI.indentLevel++;
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("meleeDistance"));
                        EditorGUILayout.Space(5);
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("meleeAnimator"));
                        EditorGUILayout.Space(5);
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("waitBetweenAttack"));
                        EditorGUI.indentLevel--;
                    }
                    EditorGUILayout.Space(5);

                    break;
                case "Debug":
                    EditorGUILayout.LabelField("DEBUG VARIABLES", EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("gunShotSfx"));
                    EditorGUILayout.Space(5);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("shootingDistanceDebug"));
                    EditorGUILayout.Space(5);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("meleeDistanceDebug"));
                    EditorGUILayout.Space(5);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("canSeePlayerDebug"));
                    EditorGUILayout.Space(5);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("searchRadiusDebug"));
                    EditorGUILayout.Space(5);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("attackRadiusDebug"));
                    EditorGUILayout.Space(5);
                    break;
            }
            EditorGUILayout.Space(10f);

            serializedObject.ApplyModifiedProperties();
        }
    }

    void OnSceneGUI()
    {
        CowsinsAI cai = (CowsinsAI)target;
        
        if(cai.searchRadiusDebug == true)
        {
            Handles.color = Color.white;
            Handles.DrawWireArc(cai.transform.position, Vector3.up, Vector3.forward, 360, cai.searchRadius);

            Vector3 searchViewAngle01 = DirectionFromAngle(cai.transform.eulerAngles.y, -cai.searchAngle / 2);
            Vector3 searchViewAngle02 = DirectionFromAngle(cai.transform.eulerAngles.y, cai.searchAngle / 2);

            Handles.color = Color.yellow;
            Handles.DrawLine(cai.transform.position, cai.transform.position + searchViewAngle01 * cai.searchRadius);
            Handles.DrawLine(cai.transform.position, cai.transform.position + searchViewAngle02 * cai.searchRadius);
        }

        if(cai.attackRadiusDebug == true)
        {
            Vector3 attackViewAngle01 = DirectionFromAngle(cai.transform.eulerAngles.y, -cai.searchAngle / 2);
            Vector3 attackViewAngle02 = DirectionFromAngle(cai.transform.eulerAngles.y, cai.searchAngle / 2);
        }
        
        if(cai.canSeePlayerDebug == true)
        {
            if (cai.canSeePlayer)
            {
                Handles.color = Color.green;
                Handles.DrawLine(cai.transform.position, cai.player.transform.position);
            }
        }
        
        if(cai.shootingDistanceDebug == true)
        {
            if (cai.inShootingDistance)
            {
                Handles.color = Color.red;
                Handles.DrawLine(cai.transform.position, cai.player.transform.position);
            }
        }

        if (cai.meleeDistanceDebug)
        {
            if (cai.inMeleeDistance)
            {
                Handles.color = Color.red;
                Handles.DrawLine(cai.transform.position, cai.player.transform.position);
            }
        }
    }

    Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}