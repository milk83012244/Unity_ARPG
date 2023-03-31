using UnityEngine;
using System.Collections;
using LitJson;

public class 工作表1Item : DataItem  {
	public int ID;
	public override int Identity(){ return ID; }
	public string Name;
	public int HP;
	public int ATK;

    public override void Setup(JsonData data) {
		base.Setup(data);
		ID = int.Parse(data["ID"].ToString());
		Name = data["Name"].ToString();
		HP = int.Parse(data["HP"].ToString());
		ATK = int.Parse(data["ATK"].ToString());

    }

	public 工作表1Item () {
	
	}
}
