using UnityEngine;
using UIText = UnityEngine.UI.Text;
using System.Collections.Generic;

/// <summary>
/// Randomly generates lines of text and adds them to a UIText element
/// </summary>
public class RandoText : MonoBehaviour
{
	// The pool of characters. Add weight by repeating characters.
	private static char[] chars = "1234567890!@#$%^&*qwertyuiopasdfghjklzxcvbn".ToCharArray();

	// The UIText object
	[SerializeField] private UIText _textObject;

	// The number of lines of history we're storing
	// set this to however many lines will fit in your display for best results
	[SerializeField] private int _maxLines = 10;

	// The baseline update interval
	[SerializeField] private float _updateRate = 1f;

	// The range of randomization of update interval, as a percentage of the update interval
	[SerializeField] private float _updateRateRandomness = 0.5f;

	// The min and max number of characters in a randomly-generated line
	[SerializeField] private int _minCharsPerLine = 5;
	[SerializeField] private int _maxCharsPerLine = 40;

	private List<string> _lines = new List<string>();
	private float _nextUpdate = 0f;

	private void Start()
	{
		_textObject = GetComponent<UIText>();
	}

	private void Update()
	{
		if (Time.time < _nextUpdate)
		{
			return;
		}

		while (_lines.Count >= _maxLines)
		{
			_lines.RemoveAt(0);
		}

		// Random.Range(int, int) excludes 2nd int
		var numChars = Random.Range(_minCharsPerLine, _maxCharsPerLine + 1);

		var line = "";

		for (int i = 0; i < numChars; i++)
		{
			line += chars[Random.Range(0, chars.Length)];
		}

		_lines.Add(line);

		_textObject.text = string.Join("\n", _lines.ToArray());

		_nextUpdate = Time.time + _updateRate * Random.Range(-_updateRateRandomness, _updateRateRandomness);
	}
}
