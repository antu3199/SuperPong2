using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetComputerDifficulty : MonoBehaviour {

    public ComputerDifficulty difficulty = ComputerDifficulty.NONE;

    public void setComputerDifficulty(){
        GameManagers.gameModel.computerDiffiulty = difficulty;
    }
}
