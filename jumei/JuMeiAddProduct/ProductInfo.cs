using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JuMeiAddProduct
{
    public class ProductInfo
    {
        private string _itemsku;
        /// <summary>
        /// 商品货号
        /// </summary>
        public string Itemsku
        {
            get { return _itemsku; }
            set { _itemsku = value; }
        }

        //<option value="485">礼品箱包</option>
        private string _category1 = "485";
        /// <summary>
        /// 一级类目
        /// </summary>
        public string Category1
        {
            get { return _category1; }
        }

        //<option value="498">单肩包/挎包/手包</option>
        private string _category2 = "498";
        /// <summary>
        /// 二级类目
        /// </summary>
        public string Category2
        {
            get { return _category2; }
        }

        //<option value="499">女包</option>
        private string _category3 = "499";   
        /// <summary>
        /// 三级类目
        /// </summary>
        public string Category3
        {
            get { return _category3; }
        }

        //<option value="501">手拎包</option>
        private string _category4 = "501";
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

        private string _imgfilepath1;
        /// <summary>
        /// 主图路径1
        /// </summary>
        public string Imgfilepath1
        {
            get { return _imgfilepath1; }
            set { _imgfilepath1 = value; }
        }

        private string _imgfilepath2;
        /// <summary>
        /// 主图路径2
        /// </summary>
        public string Imgfilepath2
        {
            get { return _imgfilepath2; }
            set { _imgfilepath2 = value; }
        }

        private string _imgfilepath3;
        /// <summary>
        /// 主图路径3
        /// </summary>
        public string Imgfilepath3
        {
            get { return _imgfilepath3; }
            set { _imgfilepath3 = value; }
        }

        private string _imgfilepath4;
        /// <summary>
        /// 主图路径4
        /// </summary>
        public string Imgfilepath4
        {
            get { return _imgfilepath4; }
            set { _imgfilepath4 = value; }
        }

        private string _imgfilepath5;
        /// <summary>
        /// 主图路径5
        /// </summary>
        public string Imgfilepath5
        {
            get { return _imgfilepath5; }
            set { _imgfilepath5 = value; }
        }

        private string _size;
        /// <summary>
        /// 尺码
        /// </summary>
        public string Size
        {
            get { return _size; }
            set { _size = value; }
        }

        private string _color;
        /// <summary>
        /// 颜色
        /// </summary>
        public string Color
        {
            get { return _color; }
            set { _color = value; }
        }

        private string _postpara;
        /// <summary>
        /// post提交过去的参数
        /// </summary>
        public string Postpara
        {
            get { return _postpara; }
            set { _postpara = value; }
        }

        private string _postresult;
        /// <summary>
        /// post提交结果
        /// </summary>
        public string Postresult
        {
            get { return _postresult; }
            set { _postresult = value; }
        }
    }
}
