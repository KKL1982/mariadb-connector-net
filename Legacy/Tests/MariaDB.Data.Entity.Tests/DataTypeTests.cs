// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published
// by the Free Software Foundation; version 3 of the License.
//
// This program is distributed in the hope that it will be useful, but
// WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
// or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public License
// for more details.
//
// You should have received a copy of the GNU Lesser General Public License along
// with this program; if not, write to the Free Software Foundation, Inc.,
// 51 Franklin St, Fifth Floor, Boston, MA 02110-1301  USA

using System;
using System.Data;
using System.Threading;
using System.Linq;
using MariaDB.Data.MySqlClient;
using NUnit.Framework;
using MariaDB.Data.MySqlClient.Tests;
using System.Data.EntityClient;
using System.Data.Common;
using System.Data.Objects;
using System.Globalization;

namespace MariaDB.Data.Entity.Tests
{
    [TestFixture]
    public class DataTypeTests : BaseEdmTest
    {
        /// <summary>
        /// Bug #45457 DbType Time is not supported in entity framework
        /// </summary>
        [Test]
        public void TimeType()
        {
            using (testEntities context = new testEntities())
            {
                TimeSpan birth = new TimeSpan(11,3,2);

                Child c = new Child();
                c.Id = 20;
                c.EmployeeID = 1;
                c.FirstName = "first";
                c.LastName = "last";
                c.BirthTime = birth;
                c.Modified = DateTime.Now;
                context.AddToChildren(c);
                context.SaveChanges();

                MySqlDataAdapter da = new MySqlDataAdapter("SELECT * FROM EmployeeChildren WHERE id=20", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                Assert.AreEqual(birth, dt.Rows[0]["birthtime"]);
            }
        }

        /// <summary>
        /// Bug #45077	Insert problem with Connector/NET 6.0.3 and entity framework
        /// Bug #45175	Wrong SqlType for unsigned smallint when generating Entity Framework Model
        /// </summary>
        [Test]
        public void UnsignedValues()
        {
            using (testEntities context = new testEntities())
            {
                var row = context.Children.First();
                context.Detach(row);
                context.Attach(row);
            }
        }

        /// <summary>
        /// Bug #44455	insert and update error with entity framework
        /// </summary>
        [Test]
        public void DoubleValuesNonEnglish()
        {
            CultureInfo curCulture = Thread.CurrentThread.CurrentCulture;
            CultureInfo curUICulture = Thread.CurrentThread.CurrentUICulture;
            CultureInfo newCulture = new CultureInfo("da-DK");
            Thread.CurrentThread.CurrentCulture = newCulture;
            Thread.CurrentThread.CurrentUICulture = newCulture;

            try
            {
                using (testEntities context = new testEntities())
                {
                    Child c = new Child();
                    c.Id = 20;
                    c.EmployeeID = 1;
                    c.FirstName = "Bam bam";
                    c.LastName = "Rubble";
                    c.BirthWeight = 8.65;
                    c.Modified = DateTime.Now;
                    context.AddToChildren(c);
                    context.SaveChanges();
                }
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = curCulture;
                Thread.CurrentThread.CurrentUICulture = curUICulture;
            }
        }

        /// <summary>
        /// Bug #46311	TimeStamp table column Entity Framework issue.
        /// </summary>
        [Test]
        public void TimestampColumn()
        {
            DateTime now = DateTime.Now;

            using (testEntities context = new testEntities())
            {
                Child c = context.Children.First();
                DateTime dt = c.Modified;
                c.Modified = now;
                context.SaveChanges();

                c = context.Children.First();
                dt = c.Modified;
                Assert.AreEqual(now, dt);
            }
        }

        /// <summary>
        /// Bug #48417	Invalid cast from 'System.String' to 'System.Guid'
        /// </summary>
        [Test]
        public void GuidType()
        {
            using (testEntities context = new testEntities())
            {
                DataTypeTest dtt = context.DataTypeTests.First();
                string guidAsChar = dtt.idAsChar;
                Assert.AreEqual(0, String.Compare(guidAsChar, dtt.id.ToString(), true));
                Assert.AreEqual(0, String.Compare(guidAsChar, dtt.id2.ToString(), true));
            }
        }
    }
}