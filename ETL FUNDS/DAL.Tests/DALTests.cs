namespace DAL.Tests
{

    public class DALTests
    {
        [Fact]
        public void When_BuildingInsertSqlString_Should_ReturnInCorrectFormat()
        {
            //Arrange
            Fund fund = new Fund()
            {
                CNPJ_FUNDO= "00.017.024/0001-53",
                DT_COMPTC = new DateTime(2019,12,01),
                VL_TOTAL = 2000000.02,
                VL_QUOTA = "999999999",
                VL_PATRIM_LIQ = 10000.43,
                CAPTC_DIA = 20000.55,
                RESG_DIA = 0,
                NR_COTST = 1
            };
            List<Fund> list = new List<Fund>();
            list.Add(fund);
            var commandMock = new Mock<IDbCommand>();
            commandMock
                .Setup(m => m.ExecuteNonQuery())
                .Verifiable();

            var connectionMock = new Mock<IDbConnection>();
            connectionMock
                .Setup(m => m.CreateCommand())
                .Returns(commandMock.Object);

            var connectionFactoryMock = new Mock<IDbConnectionFactory>();
            connectionFactoryMock
                .Setup(m => m.CreateConnection())
                .Returns(connectionMock.Object);
            var dalService = new FundDALService(connectionFactoryMock.Object);

            //Act
            var sqlString = dalService.BuildInsertSqlCommandString(list);

            //Assert
            Assert.Equal("INSERT INTO DAILY_INF (ID, CNPJ_FUNDO, DT_COMPTC, VL_TOTAL, VL_QUOTA, VL_PATRIM_LIQ, CAPTC_DIA, RESG_DIA, NR_COTST) VALUES ('0','00.017.024/0001-53','2019-12-01','2000000,02','999999999','10000,43','20000,55','0','1');", sqlString.ToString());
        }

        [Fact]
        public void When_BuildingSelectQueryWithAllParameters_Should_ReturnInCorrectFormat()
        {
            string cnpj = "00.017.024 / 0001 - 53";
            DateTime startDate = new DateTime(2017, 11, 11);
            DateTime endDate = new DateTime(2017, 11, 11);
            //arrange
            var commandMock = new Mock<IDbCommand>();
            commandMock
                .Setup(m => m.ExecuteNonQuery())
                .Verifiable();

            var connectionMock = new Mock<IDbConnection>();
            connectionMock
                .Setup(m => m.CreateCommand())
                .Returns(commandMock.Object);

            var connectionFactoryMock = new Mock<IDbConnectionFactory>();
            connectionFactoryMock
                .Setup(m => m.CreateConnection())
                .Returns(connectionMock.Object);
            var dalService = new FundDALService(connectionFactoryMock.Object);

            //Act
            var selectCommand = dalService.BuildSelectSqlCommandString(cnpj, startDate, endDate);

            //Assert
            Assert.Equal("SELECT * FROM daily_inf WHERE CNPJ_FUNDO = @CNPJ_FUNDOP AND DT_COMPTC BETWEEN @startDate AND @endDate ORDER BY DT_COMPTC ASC", selectCommand.CommandText.ToString());
        }

        [Fact]
        public void When_BuildingSelectQueryWithCNPJAndStartDateParams_Should_ReturnInCorrectFormat()
        {
            //Arrange
            string cnpj = "00.017.024 / 0001 - 53";
            DateTime startDate = new DateTime(2017, 11, 11);
            DateTime endDate = new DateTime();
            var commandMock = new Mock<IDbCommand>();
            commandMock
                .Setup(m => m.ExecuteNonQuery())
                .Verifiable();

            var connectionMock = new Mock<IDbConnection>();
            connectionMock
                .Setup(m => m.CreateCommand())
                .Returns(commandMock.Object);

            var connectionFactoryMock = new Mock<IDbConnectionFactory>();
            connectionFactoryMock
                .Setup(m => m.CreateConnection())
                .Returns(connectionMock.Object);
            var dalService = new FundDALService(connectionFactoryMock.Object);

            //Act
            var selectCommand = dalService.BuildSelectSqlCommandString(cnpj, startDate, endDate);

            //Assert
            Assert.Equal("SELECT * FROM daily_inf WHERE CNPJ_FUNDO = @CNPJ_FUNDOP AND DT_COMPTC > @startDate ORDER BY DT_COMPTC ASC", selectCommand.CommandText.ToString());
        }

        [Fact]
        public void When_BuildingSelectQueryWithCNPJAndEndDateParams_Should_ReturnInCorrectFormat()
        {
            //Arrange
            string cnpj = "00.017.024 / 0001 - 53";
            DateTime startDate = new DateTime();
            DateTime endDate = new DateTime(2020, 11, 11);
            var commandMock = new Mock<IDbCommand>();
            commandMock
                .Setup(m => m.ExecuteNonQuery())
                .Verifiable();

            var connectionMock = new Mock<IDbConnection>();
            connectionMock
                .Setup(m => m.CreateCommand())
                .Returns(commandMock.Object);

            var connectionFactoryMock = new Mock<IDbConnectionFactory>();
            connectionFactoryMock
                .Setup(m => m.CreateConnection())
                .Returns(connectionMock.Object);
            var dalService = new FundDALService(connectionFactoryMock.Object);

            //Act
            var selectCommand = dalService.BuildSelectSqlCommandString(cnpj, startDate, endDate);

            //Assert
            Assert.Equal("SELECT * FROM daily_inf WHERE CNPJ_FUNDO = @CNPJ_FUNDOP AND DT_COMPTC < @endDate ORDER BY DT_COMPTC ASC", selectCommand.CommandText.ToString());
        }
        [Fact]
        public void When_BuildingSelectQueryWithCNPJParam_Should_ReturnInCorrectFormat()
        {
            //Arrange
            string cnpj = "00.017.024 / 0001 - 53";
            DateTime startDate = new DateTime();
            DateTime endDate = new DateTime();
            var commandMock = new Mock<IDbCommand>();
            commandMock
                .Setup(m => m.ExecuteNonQuery())
                .Verifiable();

            var connectionMock = new Mock<IDbConnection>();
            connectionMock
                .Setup(m => m.CreateCommand())
                .Returns(commandMock.Object);

            var connectionFactoryMock = new Mock<IDbConnectionFactory>();
            connectionFactoryMock
                .Setup(m => m.CreateConnection())
                .Returns(connectionMock.Object);
            var dalService = new FundDALService(connectionFactoryMock.Object);

            //Act
            var selectCommand = dalService.BuildSelectSqlCommandString(cnpj, startDate, endDate);

            //Assert
            Assert.Equal("SELECT * FROM daily_inf WHERE CNPJ_FUNDO = @CNPJ_FUNDOP ORDER BY DT_COMPTC ASC", selectCommand.CommandText.ToString());
        }
        [Fact]
        public void When_BuildingSelectQueryWithoutMandatoryCnpjParam_Should_Fail()
        {
            //Arrange
            string cnpj = "";
            DateTime startDate = new DateTime();
            DateTime endDate = new DateTime();
            var commandMock = new Mock<IDbCommand>();
            commandMock
                .Setup(m => m.ExecuteNonQuery())
                .Verifiable();

            var connectionMock = new Mock<IDbConnection>();
            connectionMock
                .Setup(m => m.CreateCommand())
                .Returns(commandMock.Object);

            var connectionFactoryMock = new Mock<IDbConnectionFactory>();
            connectionFactoryMock
                .Setup(m => m.CreateConnection())
                .Returns(connectionMock.Object);
            var dalService = new FundDALService(connectionFactoryMock.Object);

            //Act
            (var fundList, var errorMessage) = dalService.SelectRecords(cnpj, startDate, endDate);

            //Assert
            Assert.Equal("The parameter CNPJ is mandatory", errorMessage);
            Assert.Empty(fundList);
        }
    }
}