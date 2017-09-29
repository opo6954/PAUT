using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PAUT
{
    public class Instruction
    {
        public string name;
        public string objective;
        public string toDo;
        public string[] toDoList;


        // if this value is negative, it means toDoList is empty and not available.
        public int toDoIdx;

        // this constructor is for Event's instruction
        public Instruction(string _name, string _objective, string _toDo)
        {
            name = _name;
            objective = _objective;
            toDo = _toDo;
        }

        // this constructor is for Asset's instruction
        public Instruction(string _name, string[] _toDoList)
        {
            name = _name;
            if (_toDoList.Length > 0)
            {
                toDoList = _toDoList;
                toDoIdx = 0;
            }
            else
                toDoIdx = -1;
        }

        public override string ToString()
        {
            return toDo;
        }
    }
}
