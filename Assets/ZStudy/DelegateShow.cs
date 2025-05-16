using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelegateShow : MonoBehaviour
{
    void Start()
    {
        Show1();
    }

    public delegate void TestDelegate();
    public TestDelegate testDelegate;

    public Action testAction;

    public delegate void TestDelegate2(int num);
    public TestDelegate2 testDelegate2;

    public Action<int> testAction2;

    public delegate void TestDelegate3(int num, string test);
    public TestDelegate3 testDelegate3;

    public Action<int, string> testAction3;

    public delegate int GetTestDelegate();
    public GetTestDelegate getTestDelegate;

    public Func<int> getTestFunc;

    public delegate int GetTestDelegate2(int num);
    public GetTestDelegate2 getTestDelegate2;

    public Func<int, int> getTestFunc2;  // ���� ��Ʈ�� �Ű�����, �ڿ� ��Ʈ�� ��ȯ��
   
    public delegate string GetTestDelegate3();
    public GetTestDelegate3 getTestDelegate3;

    public Func<string, int, string> getTestFunc3;
    // Start is called before the first frame update

    public void Show1()
    {
        // ����  �Ű����� => {�Լ�����}

        testDelegate = TestFun;

        //testDelegate?.Invoke(); // �Ʒ��� ��
        testDelegate();

        testDelegate2 = TestFun2;
        testDelegate2(77);


    }

    public void TestFun()
    {
        Debug.Log("������ �ǰ���� �Լ�");
    }

    public void TestFun2(int num)
    {
        Debug.Log("�Ű� ������ �ִ� �Լ� " + num);
    }

    public void TestFun3(string test, int num)
    {
        Debug.Log(test );
    }


    public void Shoe2()
    {
        getTestDelegate = GetTestFun1;
    }

    public int GetTestFun1()
    {
        return 7;
    }

    public int GetTestFun2(int num)
    {
        return num;
    }


    // Update is called once per frame
    void Update()
    {
        
    }

}
