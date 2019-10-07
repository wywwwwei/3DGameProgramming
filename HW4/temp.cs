public class SSAction : ScriptableObject{
    public bool enable = true;
    public bool destroy = false;

    public GameObject gameobject{get;set;}
    public Transform transform{get;set;}
    public ISSActionCallback callback{get;set;}

    protected SSAction(){}

    //Use this for initialization
    public virtual void Star(){
        throw new System.NotImplementedException();
    }

    //Update is called once per frame
    public virtual void Update(){
        throw new System.NotImplementedException();
    }
}

public class CCMoveToAction : SSAction
{
    public Vector3 target;
    public float speed;

    public static CCMoveToAction GetSSAction(Vector3 target,float speed){
        CCMoveToAction action = ScriptableObject.CreateInstance<CCMoveToAction>();
        action.target = target;
        action.speed = speed;
        return action;
    }

    public override void Update(){
        this.transform.position = Vector3.MoveTowards(this.transfprm.position,target);
        if(this.transform.position == target){
            this.destroy = true;
            this.callback.SSActionEvent(this);
        }
    }

    public override void Start(){}
}

public class CCSequenceAction : SSAction,ISSActionCallback
{
    public List<SSAction> sequence;
    public int repeat = -1;
    public int start = 0;

    public static CCSequenceAction GetSSAction(int repeat,int start,List<SSAction> sequence){
        CCSequenceAction action = ScriptableObject.CreateInstance<CCSequenceAction>();
        action.repeat = repeat;
        action.sequence = sequence;
        action.start = start;
        return action;
    }

    //Update is called once per frame
    public override void Update(){
        if(sequence.Count ==0)return;
        if(start<sequence.Count){
            sequence[start].Update();
        }
    }

    public void SSActionEvent(SSAction source,SSActionEventType events = SSActionEventType.Competeted)
    {
        source.destroy = false;
        this.start++;
        if(this.start>=sequence.Count){
            this.start = 0;
            if(repeat>0)repeat--;
            if(repeat==0){
                this.destroy = true;
                this.callback.SSActionEvent(this);
            }
        }
    }

    public override void Start(){
        foreach (SSAction action in sequence)
        {
            action.gameobject = this.gameobject;
            action.transform = this.transform;
            action.callback = this;
            action.Start();
        }
    }

    private void OnDestroy() {
        
    }
}

public enum SSActionEventType:int{Started,Competeted}

public interface ISSActionCallback
{
    void SSActionEvent(SSAction source,
    SSActionEventType events = SSActionEventType.Competeted,
    int intParam = 0,
    string strParam = null.
    Object objectParam = null);
}