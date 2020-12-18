using UnityEditor;
using UnityEngine;
using UnityFBXExporter;

public sealed class FBXExporterInRunTime : MonoBehaviour
{
	[Tooltip("Click here to export objectToExport as .fbx with materials.")]
	[SerializeField]
	bool exportFBX;
	[Tooltip("Root object of what will be exported as .fbx file.")]
	[SerializeField]
	GameObject objectToExport;
	FBXExporter fBXExporter;

	private void Start()
	{
		fBXExporter = new FBXExporter();
	}

	public void convert()
	{
		//if(exportFBX)
		//{
			exportFBX = false;

			if(objectToExport == null)
			{
				Debug.LogError($"Please assign an object to export as an .fbx file.", this);
				return;
			}

			string path = EditorUtility.SaveFilePanel($"Export {objectToExport} as .fbx", "", objectToExport.name + ".fbx", "fbx");
			Debug.Log(path);
			FBXExporter.ExportGameObjToFBX(objectToExport, path, true, true);
		//}
	}
}
