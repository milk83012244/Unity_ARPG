using UnityEngine;
using System.Collections;

/// <summary>
/// Excel讀取範例
/// </summary>
public class Example : MonoBehaviour {

    [ContextMenu("测试读")]
    void TestRead()
    {
        string excelPath = Application.dataPath + "/ExcelFile/Test1.xlsx";
        Excel xls = ExcelHelper.LoadExcel(excelPath);
        xls.ShowLog();

    
    }

    [ContextMenu("测试写")]
    void TestWrite()
    {
        string excelPath = Application.dataPath + "/ExcelFile/Test1.xlsx";
        string outputPath = Application.dataPath + "/ExcelFile/Test2.xlsx";
        Excel xls = ExcelHelper.LoadExcel(excelPath);
        

        xls.Tables[0].SetValue(2, 3, "hahha"); //設定Excel的值[工具表] SetValue(行,列,輸入值)
        xls.ShowLog();
        ExcelHelper.SaveExcel(xls, outputPath);
    }

    [ContextMenu("测试生成脚本")]
    void TestMakeCs()
    {
        string path = Application.dataPath + "/ExcelFile/Test2.xlsx";
        Excel xls = ExcelHelper.LoadExcel(path);
        ExcelDeserializer ed = new ExcelDeserializer();
        ed.FieldNameLine = 1; //指定變數名行
        ed.FieldTypeLine = 2; //指定變數類型行
        ed.FieldValueLine = 3;//指定變數值行

        ed.IgnoreSymbol = "#";
        ed.ModelPath = Application.dataPath + "/Excel4Unity/DataItem.txt";
        ed.GenerateCS(xls.Tables[0]);
    }

    

}
