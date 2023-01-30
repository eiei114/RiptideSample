using TMPro;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _nameText;
    private ushort _id;
    
    public ushort Id => _id;
    
    public int SetScore
    {
        set => _scoreText.text = value.ToString();
    }
    
    public void InitPlayerView(ushort id, string name)
    {
        _id = id;
        _nameText.text = name;
    }
}
