using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tecnicas.Mapa
{
    public struct InfMapas
    {
        public Map Mapa;
        public int aberturas;
        public bool ligado;

        public InfMapas(Map map, int aber, bool liga)
        {
            Mapa = map;
            aberturas = aber;
            ligado = false;
        }
    }

    public enum Lados { Cima, Direta, Baixo, Esquerda };

    public class Map
    {
        public Tile[,] conjTiles;
        Random rand;
        public int w;
        public int h;
        public List<Lados> ListLados = new List<Lados>();
        bool revisto = false;
        bool _Emapa = true;
        public bool IsConectada = true;

        public Map()
        {
            //Posicao = conjTiles[0, 0].position;
        }

        public Map(int posicaoX, int posicaoY, int width, int height, bool IsMapa, Random rM)
        {
            w = width;
            h = height;
            int r = 0;
            rand = rM;
            _Emapa = IsMapa;
            conjTiles = new Tile[height, width];
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    conjTiles[i, j] = new Tile(new Vector2((i + 2) * 180 + posicaoX, (j + 2) * 180 + posicaoY));
                    if (j == 0)
                        conjTiles[i, j].tipo = 4;

                    if (j == w - 1)
                        conjTiles[i, j].tipo = 5;

                    if (i == 0)
                        conjTiles[i, j].tipo = 6;

                    if (i == h - 1)
                        conjTiles[i, j].tipo = 7;

                }
            }

            conjTiles[0, 0].tipo = 0;
            conjTiles[h - 1, w - 1].tipo = 1;
            conjTiles[0, w - 1].tipo = 2;
            conjTiles[h - 1, 0].tipo = 3;

            if (IsMapa == true)
            {
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        r = rand.Next(0, 10);
                        if (i != 0 && j != 0 && j != width - 1 && i != height - 1)
                        {
                            if (r <= 8)
                            {
                                conjTiles[i, j].existe = false;
                            }
                        }
                    }
                }
            }
            else
            if (IsMapa == false)
            {
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        if (i != 0 && j != 0 && j != width - 1 && i != height - 1)
                        {
                            conjTiles[i, j].existe = false;
                        }

                        if (width > height)
                        {
                            //Esquerda
                            if (i == height / 2 && j == 0)
                            {
                                conjTiles[i, j].door = true;
                                conjTiles[i, j].existe = false;
                                conjTiles[i, j].Update();
                            }
                            //Direita
                            if (i == height / 2 && j == width - 1)
                            {
                                conjTiles[i, j].door = true;
                                conjTiles[i, j].existe = false;
                                conjTiles[i, j].Update();
                            }

                        }
                        else
                        if (width < height)
                        {
                            //Baixo
                            if (i == height - 1 && j == width / 2)
                            {
                                conjTiles[i, j].door = true;
                                conjTiles[i, j].existe = false;
                                conjTiles[i, j].Update();
                            }
                            //Cima
                            if (i == 0 && j == width / 2)
                            {
                                conjTiles[i, j].door = true;
                                conjTiles[i, j].existe = false;
                                conjTiles[i, j].Update();
                            }

                        }

                    }
                }
            }
        }

        //0 - Right, 1 - Down, 2 - Left, 3 - Down
        public Map Ponte(Tile[,] conj, int lado, Vector2 fim, int tamanho)
        {
            Random rM = new Random();
            Vector2 inicio = Vector2.Zero;
            Map bridge = new Map();
            int y = 0;
            int x = 0;
            if (lado == 0)
            {

                inicio = conj[h - 2, w / 2].position;
                x = 3;
                if (tamanho > 12)
                {
                    y = 32 - tamanho;// - 4;
                }
                else if (tamanho <= 12)
                {
                    y = 28 - (tamanho - x);
                }

                bridge = new Map((int)inicio.X, (int)inicio.Y - 540, x, y, false, rM);

                return bridge;
            }

            if (lado == 1)
            {
                inicio = conj[h / 2, w / 2].position;

                if (tamanho > 15)
                {
                    inicio.Y -= -(tamanho * 2) * 15;
                }
                else if (tamanho <= 15)
                    inicio.Y -= 340 - (tamanho * 2) - 30;

                x = Math.Abs((int)(inicio.Y - fim.Y) / 200) - 3;
                y = 3;

                bridge = new Map((int)inicio.X - 540, (int)inicio.Y + 800, x, y, false, rM);

                return bridge;
            }

            return bridge;

        }

        public void LoadImage()
        {
            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    conjTiles[i, j].LoadImage();
                }
            }
        }

        public void Update()
        {
            #region Preguiçoso
            if (revisto == false)
            {
                int width = w;
                int height = h;
                if (_Emapa == true)
                {
                    for (int i = 0; i < height; i++)
                    {
                        for (int j = 0; j < width; j++)
                        {

                            //Direita
                            if (i == height / 2 && j == width - 1 && ListLados.Contains(Lados.Direta))
                            {
                                conjTiles[i, j].door = true;
                                conjTiles[i, j].existe = false;
                                conjTiles[i, j-1].existe = false;
                                conjTiles[i, j - 2].existe = false;
                                conjTiles[i, j].Update();
                            }

                            //Baixo
                            if (i == height - 1 && j == width / 2 && ListLados.Contains(Lados.Baixo))
                            {
                                conjTiles[i, j].door = true;
                                conjTiles[i, j].existe = false;
                                conjTiles[i - 1, j].existe = false;
                                conjTiles[i - 2, j].existe = false;
                                conjTiles[i, j].Update();
                            }

                            //Esquerda
                            if (i == height / 2 && j == 0 && ListLados.Contains(Lados.Esquerda))
                            {
                                conjTiles[i, j].door = true;
                                conjTiles[i, j].existe = false;
                                conjTiles[i, j + 1].existe = false;
                                conjTiles[i, j + 2].existe = false;
                                conjTiles[i, j].Update();
                            }
                            //Cima
                            if (i == 0 && j == width / 2 && ListLados.Contains(Lados.Cima))
                            {
                                conjTiles[i, j].door = true;
                                conjTiles[i, j].existe = false;
                                conjTiles[i + 1, j].existe = false;
                                conjTiles[i + 2, j].existe = false;
                                conjTiles[i, j].Update();
                            }

                        }
                    }
                }

                if (ListLados.Count == 0)
                {
                    IsConectada = false;
                }
                revisto = true;
            }

            #endregion
        }

        virtual public void Draw()
        {
            foreach (Tile t in conjTiles)
            {
                t.Draw();
            }
        }
    }
}