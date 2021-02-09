using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    public class ProductInfo
    {
        //<option value="485">礼品箱包</option>
        private string _category1 = "485";
        /// <summary>
        /// 一级类目
        /// </summary>
        public string Category1
        {
            get { return _category1; }
        }

        //<option value="486">钱包/卡包</option>
        private string _category2 = "498";
        /// <summary>
        /// 二级类目
        /// </summary>
        public string Category2
        {
            get { return _category2; }
        }

        //<option value="499">钱包</option>
        private string _category3 = "499";   
        /// <summary>
        /// 三级类目
        /// </summary>
        public string Category3
        {
            get { return _category3; }
        }

        //<option value="500">女式钱包</option>
        private string _category4 = "500";
        /// <summary>
        /// 四级类目
        /// </summary>
        public string Category4
        {
            get { return _category4; }
        }

        //<option value="509" >卡文克莱 (CALVIN KLEIN)</option>
        private string _brand = "509";
        /// <summary>
        /// 品牌
        /// </summary>
        public string Brand
        {
            get { return _brand; }
            set { _brand = value; }
        }

        private string _title;
        /// <summary>
        /// 产品名称
        /// </summary>
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        private string _titleen;
        /// <summary>
        /// 英文名称
        /// </summary>
        public string Titleen
        {
            get { return _titleen; }
            set { _titleen = value; }
        }

        private string _spec="其他";
        /// <summary>
        /// 规格
        /// </summary>
        public string Spec
        {
            get { return _spec; }
        }

        private string _price;
        /// <summary>
        /// 海外官网价
        /// </summary>
        public string Price
        {
            get { return _price; }
            set { _price = value; }
        }

        private string _unit = "$";
        /// <summary>
        /// 价格单位
        /// </summary>
        public string Unit
        {
            get { return _unit; }
        }
    }
}
