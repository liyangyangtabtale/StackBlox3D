using System.Collections.Generic;
using UnityEngine;

public enum TetrominoType { O, L, J, T, S, Z, B, I, V, M }

public static class TetrominoData
{
    // 3x3形状定义，0=空，1=有方块
    public static readonly Dictionary<TetrominoType, int[][,]> Shapes = new Dictionary<TetrominoType, int[][,]> {
        // O型
        { TetrominoType.O, new int[][,]
            {
                new int[3,3] { {1,1,0},{1,1,0},{0,0,0} },
                new int[3,3] { {1,1,0},{1,1,0},{0,0,0} },
                new int[3,3] { {1,1,0},{1,1,0},{0,0,0} },
                new int[3,3] { {1,1,0},{1,1,0},{0,0,0} },
            }
        },
        // L型
        { TetrominoType.L, new int[][,]
            {
                new int[3,3] { {0,0,1},{1,1,1},{0,0,0} },
                new int[3,3] { {1,1,0},{0,1,0},{0,1,0} },
                new int[3,3] { {1,1,1},{1,0,0},{0,0,0} },
                new int[3,3] { {1,0,0},{1,0,0},{1,1,0} },
            }
        },
        // J型
        { TetrominoType.J, new int[][,]
            {
                new int[3,3] { {1,0,0},{1,1,1},{0,0,0} },
                new int[3,3] { {0,1,1},{0,1,0},{0,1,0} },
                new int[3,3] { {1,1,1},{0,0,1},{0,0,0} },
                new int[3,3] { {0,1,0},{0,1,0},{1,1,0} },
            }
        },
        // T型
        { TetrominoType.T, new int[][,]
            {
                new int[3,3] { {0,1,0},{1,1,1},{0,0,0} },
                new int[3,3] { {0,1,0},{0,1,1},{0,1,0} },
                new int[3,3] { {0,0,0},{1,1,1},{0,1,0} },
                new int[3,3] { {0,1,0},{1,1,0},{0,1,0} },
            }
        },
        // S型
        { TetrominoType.S, new int[][,]
            {
                new int[3,3] { {0,1,1},{1,1,0},{0,0,0} },
                new int[3,3] { {1,0,0},{1,1,0},{0,1,0} },
                new int[3,3] { {0,0,0},{0,1,1},{1,1,0} },
                new int[3,3] { {1,0,0},{1,1,0},{0,1,0} }, // 与90°相同
            }
        },
        // Z型
        { TetrominoType.Z, new int[][,]
            {
                new int[3,3] { {1,1,0},{0,1,1},{0,0,0} },
                new int[3,3] { {0,0,1},{0,1,1},{0,1,0} },
                new int[3,3] { {0,0,0},{1,1,0},{0,1,1} },
                new int[3,3] { {0,1,0},{1,1,0},{1,0,0} },
            }
        },
        // B型（2格横条）
        { TetrominoType.B, new int[][,]
            {
                new int[3,3] { {1,1,0},{0,0,0},{0,0,0} }, // 0° 横
                new int[3,3] { {0,1,0},{0,1,0},{0,0,0} }, // 90° 竖直居中
                new int[3,3] { {1,1,0},{0,0,0},{0,0,0} }, // 180° 横
                new int[3,3] { {0,1,0},{0,1,0},{0,0,0} }, // 270° 竖直居中
            }
        },
        // I型（3格横条）
        { TetrominoType.I, new int[][,]
            {
                new int[3,3] { {1,1,1},{0,0,0},{0,0,0} }, // 0° 横
                new int[3,3] { {0,1,0},{0,1,0},{0,1,0} }, // 90° 竖直居中
                new int[3,3] { {1,1,1},{0,0,0},{0,0,0} }, // 180° 横
                new int[3,3] { {0,1,0},{0,1,0},{0,1,0} }, // 270° 竖直居中
            }
        },
        // V型
        { TetrominoType.V, new int[][,]
            {
                new int[3,3] { {1,1,0},{1,0,0},{0,0,0} },
                new int[3,3] { {1,1,0},{0,1,0},{0,0,0} },
                new int[3,3] { {0,1,0},{1,1,0},{0,0,0} },
                new int[3,3] { {1,0,0},{1,1,0},{0,0,0} },
            }
        },
        // M型
        { TetrominoType.M, new int[][,]
            {
                new int[3,3] { {1,1,1},{1,0,1},{0,0,0} }, // 0° 横
                new int[3,3] { {1,1,0},{0,1,0},{1,1,0} }, // 90° 竖直居中
                new int[3,3] { {1,0,1},{1,1,1},{0,0,0} }, // 180° 横
                new int[3,3] { {0,1,1},{0,1,0},{0,1,1} }, // 270° 竖直居中
            }
        },
    };
}

public class Tetromino
{
    public TetrominoType type;
    public int rotation; // 0-3
    public int layer;    // 当前高度
    public int ring;     // 当前环向位置

    public Tetromino(TetrominoType t, int rot, int l, int r)
    {
        type = t;
        rotation = rot;
        layer = l;
        ring = r;
    }

    public int[,] GetShape()
    {
        return TetrominoData.Shapes[type][rotation];
    }
} 