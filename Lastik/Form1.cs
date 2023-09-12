using Nest;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lastik
{
    public partial class Form1 : Form
    {


        //static ConnectionSettings connection = new ConnectionSettings(new Uri("http://localhost:9200"));
        //       static ConnectionSettings settings = new ConnectionSettings(connection);
        static ConnectionSettings settings = new ConnectionSettings(new Uri("http://localhost:9200"));
        static ElasticClient client = new ElasticClient(settings);
        List<ES> es = new List<ES>();
        
        public void ReadAllDocument () 
        {

            var searchResponse = client.Search<ES>(s => s
             .Index("customer")
             .Query(q => q.MatchAll())
                        );

            List<ES> list = searchResponse.Documents.ToList();

                              dg.DataSource = list;
         
        }

        public Form1()
        {
            InitializeComponent();
            ReadAllDocument();
        }

        private void dg_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtId.Text = dg.Rows[e.RowIndex].Cells[0].Value.ToString();
            txtAd.Text = dg.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtSoy.Text = dg.Rows[e.RowIndex].Cells[2].Value.ToString();
            gizli.Text = dg.Rows[e.RowIndex].Cells[0].Value.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int ID = int.Parse(txtId.Text);
            string name = txtAd.Text;
            string soy = txtSoy.Text;
     //       List<ES> asasas = new List<ES>();
            //          asasas.Add(new ES { Id = ID , NAME = name, SOY = soy });

            var customer = new ES { Id = ID, NAME = name, SOY = soy };
            var indexResponse = client.Index(customer, i => i.Index("customer"));



            //       client.IndexMany(customer, "customer");
            ReadAllDocument();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            int id = int.Parse(txtAra.Text);


            var searchResponse = client.Search<ES>(s => s
         .Index("customer")
         .Query(q => q
         .Term(t => t.Field(f => f.Id).Value(id)))
         
     );

            List<ES> list = searchResponse.Documents.ToList();

            dg.DataSource = list;

            /*
                        var customers = searchResponse.Documents;
                        var customersText =
                            string.Join(Environment.NewLine, customers.Select(c => c.Name));
                        txtAra.Text = customersText;

                        */

        }

        private void button3_Click(object sender, EventArgs e)
        {
            int id = int.Parse(txtId.Text);
            var deleteResponse = client.Delete<ES>(id, d => d.Index("customer"));

            var searchResponse = client.Search<ES>(s => s
            .Index("customer")
            .Query(q => q.MatchAll())
                       );

            List<ES> list = searchResponse.Documents.ToList();

            dg.DataSource = list;


        }

        private void button4_Click(object sender, EventArgs e)
        {
            int id = int.Parse(txtId.Text);
            int a = int.Parse(gizli.Text);
            string adDegis = txtAd.Text;
            string SoyDegis = txtSoy.Text;

            var updateResponse = client.Update<ES>(a, u => u
            .Index("customer")
            .Doc(new ES {Id = id, NAME = adDegis, SOY = SoyDegis }));

            var searchResponse = client.Search<ES>(s => s
           .Index("customer")
           .Query(q => q.MatchAll())
                      );

            List<ES> list = searchResponse.Documents.ToList();

            dg.DataSource = list;

        }














        /*
                        var connection = new ConnectionSettings(new Uri("http://localhost:9200"));
                        var client = new ElasticClient(connection);

                        List<ES> customer = new List<ES>();
                        customer.Add(new ES { Id=1 ,Name = "Hello World" });

                        client.Indices.Create("customer",
                                        indexer => indexer.Map<ES>(
                                            x => x.AutoMap()
                                        ));

                        client.IndexMany(customer, "customer");

            */

    }
}
