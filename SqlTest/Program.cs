using System.Data;
using Microsoft.Data.SqlClient;

string sqlstring;
int indexRoom3 = 0;
int indexRoom2 = 0;
int indexRoom1 = 0;
int count1;
int count2;
int count3;
int count4;
List<string> Room3 = new List<string>();
List<string> Room2 = new List<string>();
List<string> Room1 = new List<string>();

#region 连接数据库

SqlConnectionStringBuilder scsb = new SqlConnectionStringBuilder();
scsb.DataSource = "DESKTOP-PI6SNFG";
scsb.UserID = "sa";
scsb.Password = "123";
scsb.InitialCatalog = "Test";
SqlConnection scon = new SqlConnection(scsb.ToString());
scon.Open();
if((scon.State & ConnectionState.Open) != 0)
   Console.WriteLine("连接成功");

#endregion

# region 计算房间数量
// 计算三个房间的数量
sqlstring = "USE test SELECT * FROM Table_2 WHERE 可住人数=3";
SqlCommand scom = new SqlCommand(sqlstring, scon);
SqlDataReader sdr = scom.ExecuteReader();
while (sdr.Read())
{
    Room3.Add(sdr["房间号"].ToString());
    indexRoom3++;
}
sdr.Close();
scom.Cancel();
// 计算两个房间的数量
sqlstring = "SELECT * FROM Table_2 WHERE 可住人数=2";
scom = new SqlCommand(sqlstring, scon);
sdr = scom.ExecuteReader();
while (sdr.Read())
{
    Room2.Add(sdr["房间号"].ToString());
    indexRoom2++;
}
sdr.Close();
scom.Cancel();
// 计算两个房间的数量
sqlstring = "SELECT * FROM Table_2 WHERE 可住人数=1";
scom = new SqlCommand(sqlstring, scon);
sdr = scom.ExecuteReader();
while (sdr.Read())
{
    Room1.Add(sdr["房间号"].ToString());
    indexRoom1++;
}
sdr.Close();
scom.Cancel();

#endregion

# region 主程序

while (true)
{
    int r;
    int count=0;
    Console.Write("请输入人数:");
    try
    {
        count = Convert.ToInt32(Console.ReadLine());
    }
    catch 
    {
        Console.WriteLine("请输入数字");
        continue;
    }
    sqlstring = "UPDATE [dbo].[Table_2] SET [入住人数] = null,[选择顺序] = null";
    scom = new SqlCommand(sqlstring, scon);
    r=scom.ExecuteNonQuery();
    if (r == 0)
    {
        Console.WriteLine("清空失败");
    }
    scom.Cancel();
    if (count != 0)
    {
        if (count > indexRoom3 * 3 + indexRoom2 * 2 + indexRoom1)
        {
            Console.WriteLine("人数过多");
        }
        else
        {
            count1 = count / 3;//3
            if (count1 > indexRoom3)
            {
                count1 = indexRoom3;
                count2 = count - (indexRoom3 * 3);
            }
            else
            {
                count2 = count % 3;
            }
            count3 = count2 / 2;//2
            if (count3 > indexRoom2)
            {
                count3 = indexRoom2;
                count4 = count2 - (count3 * 2);
            }
            else
            {
                count4 = count2 % 2;//1
            }

            int mark = 0;
            for (int i = 0; i < count1; i++)
            {
                sqlstring = "USE test UPDATE [dbo].[Table_2] SET [入住人数] =3,[选择顺序] = " + (i + 1) + " WHERE 房间号=" + Room3[i];
                scom = new SqlCommand(sqlstring, scon);
                r = scom.ExecuteNonQuery();
                if (r != 1)
                {
                    Console.WriteLine("G");
                }
                if (i == count1 - 1) mark = i + 1;
            }
            scom.Cancel();
            for (int i = 0; i < count3; i++)
            {
                mark = mark + 1;
                sqlstring = "USE test UPDATE [dbo].[Table_2] SET [入住人数] =2,[选择顺序] = " + mark + " WHERE 房间号=" + Room2[i];
                scom = new SqlCommand(sqlstring, scon);
                r = scom.ExecuteNonQuery();
                if (r != 1)
                {
                    Console.WriteLine("G");
                }
            }
            scom.Cancel();
            for (int i = 0; i < count4; i++)
            {
                mark = mark + 1;
                sqlstring = "USE test UPDATE [dbo].[Table_2] SET [入住人数] =1,[选择顺序] = " + mark + " WHERE 房间号=" + Room1[i];
                scom = new SqlCommand(sqlstring, scon);
                r = scom.ExecuteNonQuery();
                if (r != 1)
                {
                    Console.WriteLine("G");
                }
            }
            scom.Cancel();
            Console.WriteLine("OK");
        }
    }
}
#endregion