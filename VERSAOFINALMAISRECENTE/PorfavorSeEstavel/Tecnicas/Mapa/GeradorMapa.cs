using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tecnicas.NPC;

namespace Tecnicas.Mapa
{
    public class GeradorMapa
    {
        public List<Tile> UniversalTiles;
        public static InfMapas[] listaMapas;
        public static List<Map> listaPontes;
        Map mapa1;
        Map mapa2;
        Map mapa3;
        Map mapa4;
        Map mapa5;
        Map mapa6;
        Map mapa7;
        Map mapa8;
        Map mapa9;
        Random rand;
        //public int conexoes;

        public GeradorMapa()
        {
            //conexoes = 0;
        }

        public int Inicializa()
        {
            rand = new Random();
            // TODO: Add your initialization logic here
            #region CRIAÇAO DOS MAPAS
            listaMapas = new InfMapas[9];
            UniversalTiles = new List<Tile>();
            int posicaoGlobal = 5500;

            int x = rand.Next(10, 25);// rand.Next(10,15);
            int y = rand.Next(10, 15);// rand.Next(10, 15);
            //First Row
            mapa1 = new Map(posicaoGlobal * 0, posicaoGlobal * 0, x, y, true, rand);
            mapa2 = new Map(posicaoGlobal * 0, posicaoGlobal * 1, x, y, true, rand);
            mapa3 = new Map(posicaoGlobal * 0, posicaoGlobal * 2, x, y, true, rand);

            //Second Row
            mapa4 = new Map(posicaoGlobal * 1, posicaoGlobal * 0, x, y, true, rand);
            mapa5 = new Map(posicaoGlobal * 1, posicaoGlobal * 1, x, y, true, rand);
            mapa6 = new Map(posicaoGlobal * 1, posicaoGlobal * 2, x, y, true, rand);

            //Third Row
            mapa7 = new Map(posicaoGlobal * 2, posicaoGlobal * 0, x, y, true, rand);
            mapa8 = new Map(posicaoGlobal * 2, posicaoGlobal * 1, x, y, true, rand);
            mapa9 = new Map(posicaoGlobal * 2, posicaoGlobal * 2, x, y, true, rand);

            listaMapas[0] = new InfMapas(mapa1, 2, true);
            listaMapas[1] = new InfMapas(mapa2, 3, true);
            listaMapas[2] = new InfMapas(mapa3, 3, true);
            listaMapas[3] = new InfMapas(mapa4, 3, true);
            listaMapas[4] = new InfMapas(mapa5, 3, true);
            listaMapas[5] = new InfMapas(mapa6, 3, true);
            listaMapas[6] = new InfMapas(mapa7, 3, true);
            listaMapas[7] = new InfMapas(mapa8, 3, true);
            listaMapas[8] = new InfMapas(mapa9, 3, true);
            listaPontes = new List<Map>();
            #endregion
            int conexoes = 0;
            rand = new Random();
            int begins = rand.Next(0, 5);
            int ends = rand.Next(5, 10);
            for (int i = begins; i < ends; i++)
            {
                //[0,2] - direita [0,0] - baixo

                if (i >= 0 && i < 5 && i != 2)
                {                                                                                                       //x / 2 + 1, y - 1
                    Map x1 = listaMapas[i].Mapa.Ponte(listaMapas[i].Mapa.conjTiles, 0, listaMapas[i + 3].Mapa.conjTiles[0 + 15 - y, 0].position, y);
                    listaMapas[i].Mapa.ListLados.Add(Lados.Baixo);
                    listaMapas[i + 3].Mapa.ListLados.Add(Lados.Cima);

                    Map x2 = listaMapas[i].Mapa.Ponte(listaMapas[i].Mapa.conjTiles, 1, listaMapas[i + 1].Mapa.conjTiles[0 + (15 - y), 0].position, x);
                    listaMapas[i].Mapa.ListLados.Add(Lados.Direta);
                    listaMapas[i + 1].Mapa.ListLados.Add(Lados.Esquerda);

                    listaPontes.Add(x1);
                    listaPontes.Add(x2);
                }

                else

                if (i > 5 && i < 8)
                {
                    Map x1 = listaMapas[i].Mapa.Ponte(listaMapas[i].Mapa.conjTiles, 1, listaMapas[i + 1].Mapa.conjTiles[0, 0].position, x);
                    listaMapas[i].Mapa.ListLados.Add(Lados.Direta);
                    listaMapas[i + 1].Mapa.ListLados.Add(Lados.Esquerda);

                    listaPontes.Add(x1);
                }
                else

                if (i == 2 || i == 5)
                {
                    Map x1 = listaMapas[i].Mapa.Ponte(listaMapas[i].Mapa.conjTiles, 0, listaMapas[i + 3].Mapa.conjTiles[0 + 15 - y, 0].position, y);
                    listaMapas[i].Mapa.ListLados.Add(Lados.Baixo);
                    listaMapas[i + 3].Mapa.ListLados.Add(Lados.Cima);

                    listaPontes.Add(x1);
                }
                listaMapas[i].ligado = true;
                conexoes += 1;
            }

            for (int h = 0; h < 9; h++)
            {
                foreach (Tile t in listaMapas[h].Mapa.conjTiles)
                {
                    UniversalTiles.Add(t);
                }

            }
            foreach (Map m in listaPontes)
            {
                foreach (Tile t in m.conjTiles)
                {
                    UniversalTiles.Add(t);
                }
            }

            for (int i = 0; i < 9; i++)
            {
                listaMapas[i].Mapa.Update();
            }

            return conexoes;

        }

        public void LoadContent()
        {
            for (int i = 0; i < 9; i++)
            {
                listaMapas[i].Mapa.LoadImage();
            }
            foreach (Map m in listaPontes)
            {
                m.LoadImage();
            }
        }

        public Vector2 PoePlayer(Player p)
        {
            int i = rand.Next(0, 9);
            while (listaMapas[i].ligado != true)
            {
                i = rand.Next(0, 9);
            }

            if (listaMapas[i].Mapa.conjTiles[1, 1].existe == true)
            {
                listaMapas[i].Mapa.conjTiles[1, 1].existe = false;
                listaMapas[i].Mapa.conjTiles[1, 2].existe = false;
                listaMapas[i].Mapa.conjTiles[2, 1].existe = false;
            }

            return listaMapas[i].Mapa.conjTiles[1, 1].position;
        }

        public Vector2 PoeInimigo()
        {
            int i = rand.Next(0, 9);
            while (listaMapas[i].ligado != true)
            {
                i = rand.Next(0, 9);
            }
            int x = rand.Next(1, listaMapas[i].Mapa.w - 2);
            int y = rand.Next(1, listaMapas[i].Mapa.h - 2);
            while(y == 0 && x == 0 && x == listaMapas[i].Mapa.w - 1 && y == listaMapas[i].Mapa.h - 1)
            {
                x = rand.Next(0, listaMapas[i].Mapa.w - 1);
                y = rand.Next(0, listaMapas[i].Mapa.h - 1);
            }

            if (listaMapas[i].Mapa.conjTiles[y, x].existe == true)
            {
                listaMapas[i].Mapa.conjTiles[y, x].existe = false;
            }

            return listaMapas[i].Mapa.conjTiles[y, x].position;
        }

        public void Update()
        {

        }

        public void Draw()
        {
            for (int i = 0; i < 9; i++)
            {
                listaMapas[i].Mapa.Draw();
            }

            foreach (Map m in listaPontes)
            {
                m.Draw();
            }

            foreach (Tile t in UniversalTiles)
            {
                t.Draw();
            }
        }
    }
}
