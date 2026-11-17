using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Round", menuName = "Scriptable Objects/Round")]
public class Round : ScriptableObject
{
    public int blobCount;
    public List<Blob> blobs;
    public List<float> blobLengths;
    public int roundLength;
}
