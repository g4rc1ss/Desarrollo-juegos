using System;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Threading;

namespace PongGame;

public partial class MainWindow : Window
{
    private readonly DispatcherTimer timer = new();
    private bool teclaArriba, teclaAbajo, teclaW, teclaS;
    private bool humano;
    private int desplazamiento;
    private int velocidadBolita;
    private float dxBolita, dyBolita;
    private readonly int partes = 5;
    private readonly Random r = new();
    private int contadorAleatorio;
    private int posicionIA;
    private int puntuacionP1, puntuacionP2;
    private double despl;

    public MainWindow()
    {
        InitializeComponent();
        puntuacionP1 = 0;
        puntuacionP2 = 0;
        Iniciar();
    }

    private void Iniciar()
    {
        contadorAleatorio = 0;
        posicionIA = 2;
        humano = true;
        despl = 0;
        teclaAbajo = false;
        teclaArriba = false;
        teclaW = false;
        teclaS = false;
        // Console.WriteLine(AreaJuego.Height);
        Canvas.SetLeft(Bolita, 250);
        Canvas.SetTop(Bolita, 220);
        velocidadBolita = 4;
        dxBolita = 0;
        dyBolita = 0;
        desplazamiento = 9;
        IniciarHilo();
    }

    private void IniciarHilo()
    {
        if (!timer.IsEnabled)
        {
            timer.Interval = TimeSpan.FromMilliseconds(25); //.FromSeconds(5);
            timer.Tick += Juego;
            timer.Start();
        }
    }

    private void Juego(object sender, EventArgs e)
    {
        MoverBarras();
        MoverBolita();
    }

    private bool ComprobarColision()
    {
        if (Canvas.GetLeft(Bolita) < Canvas.GetLeft(P2) + P2.Width
            && Canvas.GetTop(Bolita) + Bolita.Height > Canvas.GetTop(P2)
            && Canvas.GetTop(Bolita) < Canvas.GetTop(P2) + P2.Height
           )
        {
            Canvas.SetLeft(Bolita, Canvas.GetLeft(P2) + P2.Width - 1);
            RebotarDcha();
            return true;
        }

        if (Canvas.GetLeft(Bolita) > Canvas.GetLeft(P1) - P2.Width
            && Canvas.GetTop(Bolita) + Bolita.Height > Canvas.GetTop(P1)
            && Canvas.GetTop(Bolita) < Canvas.GetTop(P1) + P1.Height
           )
        {
            Canvas.SetLeft(Bolita, Canvas.GetLeft(P1) - Bolita.Width + 1);
            RebotarIzda();
            return true;
        }

        return false;
    }

    private void RebotarDcha()
    {
        var partes = 5;
        var posicion = CalcularParte(partes, P2);
        double angulo = 0;
        switch (posicion)
        {
            case 0:
                angulo = Math.PI / 4;
                break;
            case 1:
                angulo = Math.PI / 8;
                break;
            case 2:
                angulo = 0;
                break;
            case 3:
                angulo = -Math.PI / 8;
                break;
            case 4:
                angulo = -Math.PI / 4;
                break;
        }

        dxBolita = velocidadBolita * (float)Math.Cos(angulo);
        dyBolita = -velocidadBolita * (float)Math.Sin(angulo);
        AumentarVelocidadBolita();
    }

    private int CalcularParte(int v, Rectangle p)
    {
        var distancia = Canvas.GetTop(Bolita) - Bolita.Height - Canvas.GetTop(p);
        var total = p.Height + 2 * Bolita.Height;
        var parte = total / v;
        var posicion = (int)Math.Round(distancia / parte);
        Console.WriteLine("Posicion: " + (posicion + 1) + "total: " + total + "PArte: " + parte + "Distancia: " +
                          distancia);
        return posicion + 1;
    }

    private void AumentarVelocidadBolita()
    {
        if (velocidadBolita <= 20)
        {
            velocidadBolita++;
        }
    }

    private void RebotarIzda()
    {
        var posicion = CalcularParte(partes, P1);
        double angulo = 0;
        switch (posicion)
        {
            case 0:
                angulo = Math.PI / 4;
                break;
            case 1:
                angulo = Math.PI / 8;
                break;
            case 2:
                angulo = 0;
                break;
            case 3:
                angulo = -Math.PI / 8;
                break;
            case 4:
                angulo = -Math.PI / 4;
                break;
        }

        dxBolita = -velocidadBolita * (float)Math.Cos(angulo);
        dyBolita = -velocidadBolita * (float)Math.Sin(angulo);
        AumentarVelocidadBolita();
    }

    private void ActualizarLabels()
    {
        LblP1.Content = puntuacionP1;
        LblP2.Content = puntuacionP2;
    }

    private void MoverBarras()
    {
        if (humano)
        {
            if (teclaArriba)
            {
                if (ComprobarLimiteSuperior(P1))
                {
                    Canvas.SetTop(P1, Canvas.GetTop(P1) - desplazamiento);
                }
            }

            if (teclaAbajo)
            {
                if (ComprobarLimiteInferior(P1))
                {
                    Canvas.SetTop(P1, Canvas.GetTop(P1) + desplazamiento);
                }
            }
        }
        else
        {
            if (contadorAleatorio % 100 == 0)
            {
                posicionIA = r.Next(partes);
                contadorAleatorio = 0;
            }

            switch (posicionIA)
            {
                case 0:
                    despl = Canvas.GetTop(Bolita);
                    break;
                case 1:
                    despl = Canvas.GetTop(Bolita) - P1.Height / partes;
                    break;
                case 2:
                    despl = Canvas.GetTop(Bolita) - 2 * P1.Height / partes;
                    ;
                    break;
                case 3:
                    despl = Canvas.GetTop(Bolita) - 3 * P1.Height / partes;
                    ;
                    break;
                case 4:
                    despl = Canvas.GetTop(Bolita) - 4 * P1.Height / partes;
                    ;
                    break;
            }

            contadorAleatorio++;
            Canvas.SetTop(P1, despl);


            if (ComprobarLimiteSuperior(P1))
            {
            }

            if (ComprobarLimiteInferior(P1))
            {
            }
        }

        if (teclaW)
        {
            if (ComprobarLimiteSuperior(P2))
            {
                Canvas.SetTop(P2, Canvas.GetTop(P2) - desplazamiento);
            }
        }

        if (teclaS)
        {
            if (ComprobarLimiteInferior(P2))
            {
                Canvas.SetTop(P2, Canvas.GetTop(P2) + desplazamiento);
            }
        }
    }

    private bool ComprobarTanto()
    {
        if (Canvas.GetLeft(Bolita) < -2)
        {
            puntuacionP2++;
            return true;
        }

        if (Canvas.GetLeft(Bolita) > AreaJuego.Width - Bolita.Width)
        {
            puntuacionP1++;
            return true;
        }

        return false;
    }

    private void MoverBolita()
    {
        if (dxBolita == 0 && dyBolita == 0)
        {
            if (r.Next(2) == 0)
            {
                dxBolita = velocidadBolita;
            }
            else
            {
                dxBolita = -velocidadBolita;
            }
        }

        if (!ComprobarColision())
        {
            if (ComprobarTanto())
            {
                Iniciar();
                ActualizarLabels();
            }
        }

        ComprobarFuera();
        Canvas.SetTop(Bolita, Canvas.GetTop(Bolita) + dyBolita);
        Canvas.SetLeft(Bolita, Canvas.GetLeft(Bolita) + dxBolita);
    }

    private void ComprobarFuera()
    {
        if (Canvas.GetTop(Bolita) + dyBolita < 0)
        {
            Canvas.SetTop(Bolita, 0);
            dyBolita = -dyBolita;
        }

        if (Canvas.GetTop(Bolita) + dyBolita > AreaJuego.Height - Bolita.Height)
        {
            Canvas.SetTop(Bolita, AreaJuego.Height - Bolita.Height);
            dyBolita = -dyBolita;
        }
    }

    private bool ComprobarLimiteInferior(Rectangle p)
    {
        //Console.WriteLine(AreaJuego.Height);
        if (Canvas.GetTop(p) + desplazamiento > AreaJuego.Height - p.Height)
        {
            Canvas.SetTop(p, AreaJuego.Height - 1 - p.Height);
            return false;
        }

        return true;
    }

    private bool ComprobarLimiteSuperior(Rectangle p)
    {
        if (Canvas.GetTop(p) - desplazamiento < 0)
        {
            Canvas.SetTop(p, 1);
            return false;
        }

        return true;
    }

    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.Up:
                teclaArriba = true;
                break;
            case Key.Down:
                teclaAbajo = true;
                break;
            case Key.W:
                teclaW = true;
                break;
            case Key.S:
                teclaS = true;
                break;
        }
    }

    private void Window_KeyUp(object sender, KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.Up:
                teclaArriba = false;
                break;
            case Key.Down:
                teclaAbajo = false;
                break;
            case Key.W:
                teclaW = false;
                break;
            case Key.S:
                teclaS = false;
                break;
        }
    }
}
