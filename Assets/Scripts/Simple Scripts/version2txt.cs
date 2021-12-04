using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class version2txt : MonoBehaviour
{
	public string text = "dev version %VER%";

	private void OnEnable()
	{
		GetComponent<Text>().text = text.Replace("%VER%", Application.version);
	}
}