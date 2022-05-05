using SchoolManageSystem.Basics.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolManageSystem.Basics.IDCode.Guid
{
    /// <summary>
    /// generate code by Guid,封装的guid
    /// </summary>
    public static class GuidCode
    {
        #region generate a guid code

        /// <summary>
        /// generate a guid code
        /// </summary>
        /// <returns>guid code</returns>
        public static System.Guid GetGuidCode()
        {
            return System.Guid.NewGuid();
        }

        #endregion

        #region generate a int64 number by guid

        /// <summary>
        /// generate a int64 number by guid
        /// </summary>
        /// <returns></returns>
        public static long GetInt64UniqueCode()
        {
            System.Guid value = System.Guid.NewGuid();
            return value.ToInt64();
        }

        /// <summary>
        /// generate a int32 number by guid
        /// </summary>
        /// <returns></returns>
        public static int GetInt32UniqueCode()
        {
            System.Guid value = System.Guid.NewGuid();
            int numOne = value.ToInt32();
            System.Guid valueTwo = System.Guid.NewGuid();
            int numTwo = valueTwo.ToInt32();
            return Math.Abs(numTwo + numOne);
        }

        #endregion

        #region generate a unique code by guid

        /// <summary>
        /// generate a unique code by guid
        /// </summary>
        /// <returns></returns>
        public static string GetUniqueCode()
        {
            System.Guid value = System.Guid.NewGuid();
            return value.ToUniqueCode();
        }

        #endregion

        #region generate a specified length code by uniquecode

        /// <summary>
        /// generate a specified length code by uniquecode,too ensure uniqueness,recommend to set length at least 16
        /// </summary>
        /// <param name="length">code length</param>
        /// <param name="prefix">prefix</param>
        /// <returns>unique code</returns>
        public static string GetUniqueCode(int length, string prefix = "")
        {
            string uniqueCode = prefix + GetUniqueCode().ToUpper();
            if (uniqueCode.Length == length)
            {
                return uniqueCode;
            }
            if (uniqueCode.Length > length)
            {
                uniqueCode = uniqueCode.Substring(0, length);
            }
            else
            {
                string codeString = "1a2b3c4d5e6f7g8h9i0j9k8l7m6n5o4p3q2r1s0t1u2v3w4x5y6z".ToUpper();
                int randomNum = length - uniqueCode.Length;
                int randomLength = codeString.Length;
                System.Random random = new System.Random();
                for (var r = 0; r < randomNum; r++)
                {
                    int csnum = random.Next(randomLength);
                    uniqueCode += codeString[csnum];
                }
            }
            return uniqueCode;
        }

        #endregion
    }
}
