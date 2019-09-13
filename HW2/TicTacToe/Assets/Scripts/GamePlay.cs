using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlay
{
    public enum Status {Empty,Player1,Player2};
    public enum Result {Gaming,Draw,Win,Lose};

    private Status[,] gridMap= new Status[3,3]; 
    private bool isPlayer1;
    private bool Ingame;

    public void restart()
    {
        isPlayer1 = true;
        Ingame = true;
        for(int i = 0;i<3;i++)
        {
            for(int j = 0;j<3;j++)
            {
                setMap(i,j,Status.Empty);
            }
        }
    }

    public bool ifIngame()
    {
        return Ingame;
    }

    public void setIngame(bool temp)
    {
        Ingame = temp;
    }

    public Result ifWin()
    {
        Result temp;
        for(int i = 0;i<3;i++)
        {
            temp = checkRow(i);
            if(temp!=Result.Gaming)return temp;
            temp = checkCol(i);
            if(temp!=Result.Gaming)return temp;
        }
        temp = checkCross();
        if(temp!=Result.Gaming)return temp;
        if(checkFull()==Result.Draw)return Result.Draw;
        return Result.Gaming;
    }

    public Result checkFull()
    {
        for(int i = 0;i<3;i++)
        {
            for(int j = 0;j<3;j++)
            {
                if(gridMap[i,j]==Status.Empty)
                    return Result.Gaming;
            }
        }
        return Result.Draw;
    }

    public Result checkRow(int i)
    {
        if(gridMap[i,0]!=Status.Empty&&gridMap[i,0]==gridMap[i,1]&&gridMap[i,1]==gridMap[i,2])
            return (gridMap[i,0]==Status.Player1?Result.Win:Result.Lose);
        return Result.Gaming; 
    }

    public Result checkCol(int i)
    {
        if(gridMap[0,i]!=Status.Empty&&gridMap[0,i]==gridMap[1,i]&&gridMap[1,i]==gridMap[2,i])
            return (gridMap[0,i]==Status.Player1?Result.Win:Result.Lose);
        return Result.Gaming; 
    }

    public Result checkCross()
    {
        if(gridMap[0,0]!=Status.Empty&&gridMap[0,0]==gridMap[1,1]&&gridMap[1,1]==gridMap[2,2])
            return (gridMap[0,0]==Status.Player1?Result.Win:Result.Lose);
        if(gridMap[0,2]!=Status.Empty&&gridMap[0,2]==gridMap[1,1]&&gridMap[1,1]==gridMap[2,0])
            return (gridMap[0,2]==Status.Player1?Result.Win:Result.Lose);
        return Result.Gaming; 
    }

    public Status getMap(int x,int y)
    {
        return gridMap[x,y];
    }

    public void setMap(int x,int y,Status cur)
    {
        gridMap[x,y] = cur;
    }

    public void setFirst(bool isFirst)
    {
        isPlayer1 = isFirst;
    } 

    public bool getTurn()
    {
        return isPlayer1;
    }

    public void autoOperate()
    {
        if(checkFull()==Result.Draw)return;
        setFirst(true);
        if(toAttack())return;
        if(toDefend())return;
        randomOperate();
        return;
    }

    public bool findPos(Status Player)
    {
        for(int i = 0;i<3;i++)
        {
            if(gridMap[i,0]==Status.Empty&&gridMap[i,1]==Player&&gridMap[i,2]==Player)
            {
                gridMap[i,0]=Status.Player2;
                return true;
            }
            else if(gridMap[i,1]==Status.Empty&&gridMap[i,0]==Player&&gridMap[i,2]==Player)
            {
                gridMap[i,1]=Status.Player2;
                return true;
            }
            else if(gridMap[i,2]==Status.Empty&&gridMap[i,0]==Player&&gridMap[i,1]==Player)
            {
                gridMap[i,2]=Status.Player2;
                return true;
            }
        }
        for(int i = 0;i<3;i++)
        {
            if(gridMap[0,i]==Status.Empty&&gridMap[1,i]==Player&&gridMap[2,i]==Player)
            {
                gridMap[0,i]=Status.Player2;
                return true;
            }
            else if(gridMap[1,i]==Status.Empty&&gridMap[0,i]==Player&&gridMap[2,i]==Player)
            {
                gridMap[1,i]=Status.Player2;
                return true;
            }
            else if(gridMap[2,i]==Status.Empty&&gridMap[0,i]==Player&&gridMap[1,i]==Player)
            {
                gridMap[2,i]=Status.Player2;
                return true;
            }
        }
        if(gridMap[1,1]==Status.Empty)
        {
            if((gridMap[0,0]==Player&&gridMap[2,2]==Player)||(gridMap[0,2]==Player&&gridMap[2,0]==Player))
            {
                gridMap[1,1]=Status.Player2;
                return true;
            }
        }
        else if(gridMap[1,1]==Player)
        {
            if(gridMap[0,0]==Status.Empty&&gridMap[2,2]==Player)
            {
                gridMap[0,0] = Status.Player2;
                return true;
            }
            else if(gridMap[0,0]==Player&&gridMap[2,2]==Status.Empty)
            {
                gridMap[2,2] = Status.Player2;
                return true;
            }
            else if(gridMap[0,2]==Player&&gridMap[2,0]==Status.Empty)
            {
                gridMap[2,0] = Status.Player2;
                return true;
            }
            else if(gridMap[0,2]==Status.Empty&&gridMap[2,0]==Player)
            {
                gridMap[0,2] = Status.Player2;
                return true;
            }
        }
        return false;
    }
    public bool toAttack()
    {
        return findPos(Status.Player2);
    }

    public bool toDefend()
    {
        return findPos(Status.Player1);
    }

    public void randomOperate()
    {
        int x =  UnityEngine.Random.Range(0, 2);
        int y =  UnityEngine.Random.Range(0, 2);
        while(gridMap[x,y]!=Status.Empty)
        {
            x =  UnityEngine.Random.Range(0, 2);
            y =  UnityEngine.Random.Range(0, 2);
        }
        gridMap[x,y] = Status.Player2;
    }
}
