using UnityEngine;
using UnityEngine.UI;

public class RefreshRate2txt : MonoBehaviour
{
	private Text text;

	private void Awake()
	{
		text = GetComponent<Text>();
	}

	private void Update()
	{
		text.text = 1 / Time.unscaledDeltaTime + "fps";
	}
}