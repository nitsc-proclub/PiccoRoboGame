using UnityEngine;
using UnityEngine.UI;

public class DataController : MonoBehaviour
{
    // �v������
    // public float TimeCounte { get; private set; } = 0f;

    // �J�n���ꂽ���ǂ����̃t���O
    public bool IsStarted { get; set; } = false;

    // �o�ߎ��Ԃ�\������e�L�X�g�I�u�W�F�N�g
    public GameObject DataText { get; private set; }

    void Start()
    {
        // �e�L�X�g�I�u�W�F�N�g���擾���Ă���
        this.DataText = GameObject.Find("Data");

        // �v���J�n
        this.IsStarted = true;
    }

    void Update()
    {
        if (this.IsStarted)
        {
            // �^�C�}�[���v���J�n���̏ꍇ�̂�
            // �o�ߎ��Ԃ������A�e�L�X�g�ɕ\��
            //this.TimeCounte += Time.deltaTime;
            //this.TimerText.GetComponent<Text>().text = this.TimeCounte.ToString("F2");
        }
    }
}
