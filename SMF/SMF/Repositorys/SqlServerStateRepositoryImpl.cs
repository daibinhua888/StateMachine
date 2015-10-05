using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMFramework.Repositorys
{
    class SqlServerStateRepositoryImpl : IStateRepository
    {
        /*
            StateMachineType	NVARCHAR(300)	NOT NULL
            RelatedID		    NVARCHAR(500)	NOT NULL
            StateName		    NVARCHAR(300)	NOT NULL
            UpdatedOn		    DATETIME	NOT NULL
        */
        public string Find(string stateMachineType, string relatedId)
        {
            using (SqlConnection con = new SqlConnection(Configs.Config.RepositoryConnectionString))
            {
                string sql = "SELECT TOP 1 [StateName] FROM [FR_StateMachineRecords](NOLOCK) WHERE [StateMachineType]=@StateMachineType AND [RelatedID]=@RelatedID";
                SqlCommand com = new SqlCommand(sql);

                com.Parameters.Add(new SqlParameter("@StateMachineType", stateMachineType));
                com.Parameters.Add(new SqlParameter("@RelatedID", relatedId));

                com.Connection = con;

                con.Open();

                using (var reader = com.ExecuteReader())
                {
                    if (reader.Read())
                        return reader.GetString(0);
                }

                con.Close();
            }

            return null;
        }

        public void Save(string stateMachineType, string relatedId, string stateName)
        {
            using (SqlConnection con = new SqlConnection(Configs.Config.RepositoryConnectionString))
            {
                string sql = @"

                                IF EXISTS   (
                                                SELECT  1 
                                                FROM    [FR_StateMachineRecords]
                                                WHERE   [StateMachineType]=@StateMachineType 
                                                  AND   [RelatedID]=@RelatedID
                                            )
                                BEGIN
                                    UPDATE  [FR_StateMachineRecords]
                                    SET     [StateName]=@StateName,
                                            [UpdatedOn]=GETDATE()
                                    WHERE   [StateMachineType]=@StateMachineType 
                                      AND   [RelatedID]=@RelatedID
                                END
                                ELSE
                                BEGIN
                                    INSERT INTO [FR_StateMachineRecords](StateMachineType, RelatedID, StateName, UpdatedOn)
                                    VALUES(@StateMachineType, @RelatedID, @StateName, GETDATE())
                                END

";
                SqlCommand com = new SqlCommand(sql);

                com.Parameters.Add(new SqlParameter("@StateMachineType", stateMachineType));
                com.Parameters.Add(new SqlParameter("@RelatedID", relatedId));
                com.Parameters.Add(new SqlParameter("@StateName", stateName));

                com.Connection = con;

                con.Open();

                com.ExecuteNonQuery();

                con.Close();
            }
        }
    }
}
