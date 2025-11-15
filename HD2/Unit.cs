using ItemStatsSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace Helldiver
{
    public static class Unit
    {
        public static bool SetPrivateField(this Item item, string fieldName, object value)
        {
            FieldInfo field = typeof(Item).GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            bool flag = field != null;
            bool result;
            if (flag)
            {
                field.SetValue(item, value);
                result = true;
            }
            else
            {
                FieldInfo[] fields = typeof(Item).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                string text = string.Join(", ", Array.ConvertAll<FieldInfo, string>(fields, (FieldInfo f) => f.Name));
                Debug.LogWarning(string.Concat(new string[]
                {
                "[ItemExtensions] 未找到字段 '",
                fieldName,
                "'，可能结构已变。\n可用字段列表: [",
                text,
                "]"
                }));
                result = false;
            }
            return result;
        }
        public static List<GameObject> GetAllChildren(GameObject parent, bool includeInactive = true)
        {
            if (parent == null) return new List<GameObject>();

            // 获取所有层级的Transform（包括自身）
            Transform[] allTransforms = parent.GetComponentsInChildren<Transform>(includeInactive);

            List<GameObject> children = new List<GameObject>();
            foreach (Transform t in allTransforms)
            {
                if (t.gameObject != parent) // 过滤掉父物体自身
                {
                    children.Add(t.gameObject);
                }
            }
            return children;
        }
        public static void PrintAllComponents(GameObject target, bool excludeTransform = false)
        {
            if (target == null)
            {
                Debug.LogWarning("目标GameObject为空！");
                return;
            }

            // 获取所有Component
            Component[] allComponents = target.GetComponents<Component>();

            Debug.Log($"开始打印 {target.name} 的所有Component（共 {allComponents.Length} 个）:");

            foreach (Component comp in allComponents)
            {
                // 可选：排除Transform组件
                if (excludeTransform && comp is Transform) continue;

                // 打印组件名称
                Debug.Log($"- {comp.GetType().Name}");
            }
        }
        public static void PrintMeshRendererMaterials(GameObject target, bool printDetails = true)
        {
            if (target == null)
            {
                Debug.LogWarning("目标GameObject为空！");
                return;
            }

            // 获取目标物体上的MeshRenderer组件
            MeshRenderer meshRenderer = target.GetComponent<MeshRenderer>();
            if (meshRenderer == null)
            {
                Debug.LogWarning($"目标物体 {target.name} 上未找到MeshRenderer组件！");
                return;
            }

            // 获取MeshRenderer的材质数组
            Material[] materials = meshRenderer.materials;
            if (materials == null || materials.Length == 0)
            {
                Debug.Log($"MeshRenderer {meshRenderer.name} 没有材质！");
                return;
            }

            Debug.Log($"开始打印 {target.name} 的MeshRenderer材质（共 {materials.Length} 个）:");

            // 遍历并打印每个材质的信息
            for (int i = 0; i < materials.Length; i++)
            {
                Material mat = materials[i];
                if (mat == null) continue;

                Debug.Log($"材质 [{i}]:");
                Debug.Log($"  - 名称: {mat.name}");
                Debug.Log($"  - 实例ID: {mat.GetInstanceID()}");

                if (printDetails)
                {
                    Debug.Log($"  - 主纹理: {mat.mainTexture?.name ?? "无"}");
                    Debug.Log($"  - 基础颜色: {mat.color}");
                    Debug.Log($"  - 着色器: {mat.shader.name}");
                }
            }
        }
        public static void PrintMeshFilterInfo(GameObject target, bool printDetailedData = false)
        {
            if (target == null)
            {
                Debug.LogWarning("目标GameObject为空！");
                return;
            }

            // 获取目标物体上的MeshFilter组件
            MeshFilter meshFilter = target.GetComponent<MeshFilter>();
            if (meshFilter == null)
            {
                Debug.LogWarning($"目标物体 {target.name} 上未找到MeshFilter组件！");
                return;
            }

            // 获取网格
            Mesh mesh = meshFilter.sharedMesh;
            if (mesh == null)
            {
                Debug.Log($"MeshFilter {meshFilter.name} 未分配网格！");
                return;
            }

            Debug.Log($"开始打印 {target.name} 的MeshFilter网格信息（网格名称：{mesh.name}）:");

            // 打印信息
            Debug.Log($"- 网格类型: {(mesh.isReadable ? "可读" : "不可读")}");
            Debug.Log($"- 顶点数量: {mesh.vertexCount}");
            Debug.Log($"- 法线是否存在: {mesh.normals.Length > 0}");
            Debug.Log($"- 切线是否存在: {mesh.tangents.Length > 0}");
            Debug.Log($"- 包含蒙皮信息: {mesh.bindposes.Length > 0}（骨骼动画网格）");
        }
        /// <summary>
        /// 验证资源是否加载成功
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="resourceName"></param>
        public static void ValidateResource(object resource,string resourceName)
        {
            if (resource == null)
            {
                Debug.LogError($"失败：{resourceName}");
            }
            else
            {
                string logMessage = $"成功：{resourceName}";

                Debug.Log(logMessage);
            }
        }

    }
}
