using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AutoMove
{
    private Priest[] objPriest;
    private Devil[] objDevil;
    private Boat boat;
    public int state = 0b01111111;
    public List<int> result = new List<int>();
    public int[] maps = new int[]{0,1,1,2,1,2,2,3};
    private bool findAnswer = false;
    private bool[] visited ; 

    public AutoMove(Priest[] objPriest,Devil[] objDevil,Boat boat)
    {
        this.objPriest = objPriest;
        this.objDevil = objDevil;
        this.boat = boat;
        visited = new bool[0x80];
    }

    public bool checkState(int curState){
        int priestLeftNum = getPriestLeft(curState);
        int priestRightNum = getPriestRight(curState);
        int devilLeftNum = getDevilLeft(curState);
        int devilRightNum = getDevelRight(curState);
        if(priestLeftNum!=0&&priestLeftNum<devilLeftNum)return false;
        if(priestRightNum!=0&&priestRightNum<devilRightNum)return false;
        return true;
    }

    public bool getBoatState(int curState){
        return Convert.ToBoolean(curState & 0x40);
    }

    public int getPriestRight(int curState){
        return maps[((curState & 0x38) >> 3)];
    }

    public int getPriestLeft(int curState){
        return maps[((curState & 0x38)>>3)^0x07];
    }

    public int getDevelRight(int curState){
        return maps[(curState & 0x07)];
    }

    public int getDevilLeft(int curState){
        return maps[(curState & 0x07) ^ 0x07];
    }

    public int movePriestOrDevil(int curState,int num,bool Priest){
        int newState = curState;
        int count = 0;
        int mask = Priest ? 0x20 : 0x04;
        if(Convert.ToBoolean(curState & 0x40)){
            newState &= ~(0x40);
            for(int i = 0;i<3;i++){
                if(Convert.ToBoolean(curState&mask)){
                    newState &= ~(mask);
                    count++;
                }
                if(count == num){
                    return newState;
                }
                mask = mask >> 1;
            }
            
        }else{
            newState |= 0x40;
            for(int i = 0;i<3;i++){
                if(Convert.ToBoolean((~curState) & mask)){
                    newState |= mask;
                    count++;
                }
                if(count == num){
                    return newState;
                }
                mask = mask >> 1;
            }
        }
        return 0b10000000;
    }

    public int movePriestAndDevil(int curState){
        int newState = 0;
        newState |= (movePriestOrDevil(curState,1,true) & 0xB8);
        newState |= (movePriestOrDevil(curState,1,false) & 0x87);
        if(Convert.ToBoolean(curState & 0x40)){
            newState &= 0xBF;
        }else{
            newState |= 0x40;
        }
        if(Convert.ToBoolean(newState & 0x80)){
            //Debug.Log("New Err");
            return 0b10000000;
        }
        return newState;
    }

    public void dfs(int curState){
        if(curState == 0b00000000){
            result.Add(curState);
            findAnswer = true;
            return;
        }
        visited[curState] = true;
        int newState1 = movePriestOrDevil(curState,1,true);
        int newState2 = movePriestOrDevil(curState,2,true);
        int newState3 = movePriestOrDevil(curState,1,false);
        int newState4 = movePriestOrDevil(curState,2,false);
        int newState5 = movePriestAndDevil(curState);
        //Debug.Log("NewState:"+newState1+":"+newState2+":"+newState3+":"+newState4+":"+newState5);
        if(newState1 != 0b10000000 && !visited[newState1]){
            visited[newState1] = true;
            if(checkState(newState1))
                dfs(newState1);
            if(findAnswer){
                result.Add(newState1);
                return;
            }
        }
        if(newState2 != 0b10000000 && !visited[newState2]){
            visited[newState2] = true;
            if(checkState(newState2))
                dfs(newState2);
            if(findAnswer){
                result.Add(newState2);
                return;
            }
        }
        if(newState3 != 0b10000000 && !visited[newState3]){
            visited[newState3] = true;
            if(checkState(newState3))
                dfs(newState3);
            if(findAnswer){
                result.Add(newState3);
                return;
            }
        }
        if(newState4 != 0b10000000 && !visited[newState4]){
            visited[newState4] = true;
            if(checkState(newState4))
                dfs(newState4);
            if(findAnswer){
                result.Add(newState4);
                return;
            }
        }
        if(newState5 != 0b10000000 && !visited[newState5]){
            visited[newState5] = true;
            if(checkState(newState5)){
                dfs(newState5);
                return;
            }
            if(findAnswer){
                result.Add(newState5);
                return;
            }
        }
        return;      
    }

    public void updateState(){
        state = 0;
        for(int i = 0;i < 3;i++)
        {
            state = state<<1;
            if(objPriest[i].position == Character.Status.leftBoat||objPriest[i].position == Character.Status.leftLand)
                state+=0;
            else
                state+=1;
        }
        for(int i = 0;i < 3;i++)
        {
            state = state<<1;
            if(objDevil[i].position == Character.Status.leftBoat||objDevil[i].position == Character.Status.leftLand)
                state+=0;
            else
                state+=1;
        }
        state |= boat.isLeft ? 0 : 0x40;
        Debug.Log("Be called"+state);
    }

    public int AIMove(){
        updateState();
        if(state == 0){
            return 0;
        }
        if(result.Count > 0){
            int cmp = result[result.Count-1];
            int statePriest = maps[((state & 0x38)>>3)];
            int stateDevil = maps[state&0x07];
            int nextPriest = maps[((cmp & 0x38)>>3)];
            int nextDevil = maps[cmp&0x07];
            if(statePriest == nextPriest && stateDevil == nextDevil){
                result.RemoveAt(result.Count-1);
                Debug.Log("current Reuslt:"+result[result.Count-1]);
                int res = result[result.Count-1] ;
                nextPriest = maps[(res&0x38)>>3];
                nextDevil = maps[res & 0x07];
                return (nextPriest - statePriest > 0?nextPriest - statePriest:-(nextPriest - statePriest))*10+(nextDevil-stateDevil>0?nextDevil-stateDevil:-(nextDevil-stateDevil));
            }
        }
        for(int i = 0;i<0x80;i++){
            visited[i] = false;
        }
        result.Clear();
        findAnswer = false;
        dfs(state);
        Debug.Log("Over");
        if(findAnswer == false){
            return 0b100000000;
        }
        Debug.Log("result:"+result[result.Count-1]);
        int change = (result[result.Count-1] & 0x3F)^(state & 0x3F);
        return maps[change>>3]*10+maps[change & 0x07];
    }
}
