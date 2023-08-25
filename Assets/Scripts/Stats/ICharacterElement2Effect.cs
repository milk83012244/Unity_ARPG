using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �@�q�ݩ�2���ĪG������k
/// </summary>
public interface ICharacterElement2Effect
{
    /// <summary>
    /// �Ұ�2���ݩʮĪG
    /// </summary>
    public void StartElementStatus2(ElementType elementType);

    /// <summary>
    /// �i�A���i�J2�����N�o�ɶ�
    /// </summary>
    public IEnumerator Status2CoolDown(ElementType elementType);

    /// <summary>
    /// ��2�� �U�N�ĪG��@
    /// </summary>
    public IEnumerator FireElementStatus2();
    /// <summary>
    /// ��2���y���ˮ`��@
    /// </summary>
    public IEnumerator FireElementStatus2Damage(PlayerCharacterStats playerGiver = null, OtherCharacterStats otherGiver = null);

    /// <summary>
    /// �B2�� �B��ĪG��@
    /// </summary>
    public IEnumerator IceElementStatus2();

    /// <summary>
    /// ��2�� �����ĪG��@
    /// </summary>
    public IEnumerator WindElementStatus2();
    /// <summary>
    /// ��2���y���ˮ`��@
    /// </summary>
    public IEnumerator WindElementStatus2Damage(PlayerCharacterStats playerGiver=null, OtherCharacterStats otherGiver = null);

    /// <summary>
    /// �p2�� �·��ĪG��@
    /// </summary>
    public IEnumerator ThunderElementStatus2();
    /// <summary>
    /// �p2���y���ˮ`��@
    /// </summary>
    public IEnumerator ThunderElementStatus2Damage(PlayerCharacterStats playerGiver = null, OtherCharacterStats otherGiver = null);

    /// <summary>
    /// �M������ɶ�����{
    /// </summary>
    public void Status2ActiveCorReset(ElementType elementType);

    /// <summary>
    /// �Ұ�2���V�X�ĪG
    /// </summary>
    public void StartElementStatus2MixEffect(ElementType elementType);

    /// <summary>
    /// 2���V�XĲ�o�X���ĪG
    /// </summary>
    public IEnumerator ElementStatus2MixDiffusionTrigger(ElementType elementType);

    /// <summary>
    /// �j��Ѱ�2�����A
    /// </summary>
    public void StopStatus2(ElementType elementType);
}
