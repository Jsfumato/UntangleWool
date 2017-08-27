using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VertexData : MonoBehaviour
{
    public int _vertexId;
    public List<int> _linkedId = new List<int>();

    //public byte[] GetBytesData()
    //{
    //    List<byte[]> byteBuffer = new List<byte[]>();
    //    int _totalBytesCount = 0;

    //    byte[] idBytes = System.BitConverter.GetBytes(_vertexId);
    //    _totalBytesCount += idBytes.Length;

    //    for (int i = 0; i < _linkedId.Count; ++i)
    //    {
    //        byte[] linkedVertexData = System.BitConverter.GetBytes(_vertexId);
    //        _totalBytesCount += linkedVertexData.Length;
    //    }

    //    byte[] ret = new byte[_totalBytesCount];
    //    for(int i = 0; i < byteBuffer.Count; ++i)
    //    {
    //        System.Buffer.BlockCopy()
    //        System.BitConverter.
    //        _totalBytesCount
    //    }
    //}
}
