using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic.PowerPacks;

namespace pg_ludoleague
{

    public partial class ludoLeagueForm : Form
    {
        //ROLL = 1, Blue, Red, Green, Yellow
        int mode = 0;// 2 Player mode
        int[] moveSequence = new int[2];
        path[] rpath = new path[57];
        path[] bpath = new path[57];
        path[] gpath = new path[57];
        path[] ypath = new path[57];
        path[] apath = new path[57];
        Button[] R = new Button[5];
        Button[] B = new Button[5];
        Button[] Y = new Button[5];
        Button[] G = new Button[5];
        bead[] bbead = new bead[5];
        bead[] gbead = new bead[5];
        int[] fmove = new int[5];
        Microsoft.VisualBasic.PowerPacks.OvalShape[] OS = new Microsoft.VisualBasic.PowerPacks.OvalShape[9];
        int Val = 0, ROLL = -1, dr = 1, numbbeadsopen = 0, numgbeadsopen = 0, osl = 0;
        Random Q;

        RectangleShape[] movePath = new RectangleShape[53];
        RectangleShape[] redMovePath = new RectangleShape[6];
        RectangleShape[] greenMovePath = new RectangleShape[6];
        RectangleShape[] yellowMovePath = new RectangleShape[6];
        RectangleShape[] blueMovePath = new RectangleShape[6];

        public ludoLeagueForm()
        {
            for (int i = 0; i <= 56; i++)
            {
                rpath[i] = new path();
                bpath[i] = new path();
                gpath[i] = new path();
                ypath[i] = new path();
                // apath[i] = new path();
            }
            for (int i = 0; i <= 4; i++)
            {
                B[i] = new Button();
                G[i] = new Button();
                R[i] = new Button();
                Y[i] = new Button();
                bbead[i] = new bead();
                gbead[i] = new bead();
            }
            for (int i = 0; i <= 4; i++)
            {
                fmove[i] = 0;
            }
            for (int i = 0; i <= 8; i++)
            {
                OS[i] = new Microsoft.VisualBasic.PowerPacks.OvalShape();
            }

            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            R[1].BringToFront();
            rectangleShapeDice.SendToBack();

            for (int mvar = 1; mvar <= 52; )
            {
                movePath[mvar] = new RectangleShape();
                int rs_index = shapeContainer1.Shapes.IndexOfKey("rectangleShape" + mvar);
                if (rs_index != -1)
                {
                    movePath[mvar] = (RectangleShape)shapeContainer1.Shapes.get_Item(rs_index);
                    if (!(movePath[mvar] == null))
                    {
                        //blue
                        bpath[mvar - 1].x = movePath[mvar].Left; //bpath[0], bpath[50]
                        bpath[mvar - 1].y = movePath[mvar].Top;

                        //redValues
                        if (mvar < 14)
                        {
                            rpath[mvar + 38].x = movePath[mvar].Left; //rpath[0], rpath[50] //1 - > 52 //-3
                            rpath[mvar + 38].y = movePath[mvar].Top;
                            //textBox3.Text += "" + (mvar + 38) + "," + rpath[mvar + 38].x + ", " + rpath[mvar + 38].y + "\r\n";
                        }
                        else
                        {
                            rpath[mvar - 14].x = movePath[mvar].Left; //rpath[0], rpath[50] //1 - > 52
                            rpath[mvar - 14].y = movePath[mvar].Top;
                            //textBox3.Text += "" + (mvar - 14) + "," + rpath[mvar - 14].x + ", " + rpath[mvar - 14].y + "\r\n";
                        }

                        //greenValues
                        if (mvar < 27)
                        {
                            gpath[mvar + 25].x = movePath[mvar].Left; //gpath[0], gpath[50] //1 - > 52
                            gpath[mvar + 25].y = movePath[mvar].Top;
                        }
                        else
                        {
                            gpath[mvar - 27].x = movePath[mvar].Left; //gpath[0], gpath[50] //1 - > 52
                            gpath[mvar - 27].y = movePath[mvar].Top;
                        }

                        //yellowValues
                        if (mvar < 40)
                        {
                            ypath[mvar + 12].x = movePath[mvar].Left; //ypath[0], rypath[50] //1 - > 52
                            ypath[mvar + 12].y = movePath[mvar].Top;
                        }
                        else
                        {
                            ypath[mvar - 40].x = movePath[mvar].Left; //ypath[0], ypath[50] //1 - > 52
                            ypath[mvar - 40].y = movePath[mvar].Top;
                        }

                    }
                }
                mvar++;
            }

            for (int mvar = 1; mvar <= 5; )
            {
                blueMovePath[mvar] = new RectangleShape();
                redMovePath[mvar] = new RectangleShape();
                greenMovePath[mvar] = new RectangleShape();
                yellowMovePath[mvar] = new RectangleShape();
                int rs_indexB = shapeContainer1.Shapes.IndexOfKey("rectangleShapeBH" + mvar);
                int rs_indexR = shapeContainer1.Shapes.IndexOfKey("rectangleShapeRH" + mvar);
                int rs_indexG = shapeContainer1.Shapes.IndexOfKey("rectangleShapeGH" + mvar);
                int rs_indexY = shapeContainer1.Shapes.IndexOfKey("rectangleShapeYH" + mvar);
                if (rs_indexB != -1)
                {
                    blueMovePath[mvar] = (RectangleShape)shapeContainer1.Shapes.get_Item(rs_indexB);
                    redMovePath[mvar] = (RectangleShape)shapeContainer1.Shapes.get_Item(rs_indexR);
                    greenMovePath[mvar] = (RectangleShape)shapeContainer1.Shapes.get_Item(rs_indexG);
                    yellowMovePath[mvar] = (RectangleShape)shapeContainer1.Shapes.get_Item(rs_indexY);
                    if (!(blueMovePath[mvar] == null))
                    {
                        bpath[mvar + 50].x = blueMovePath[mvar].Left; //bpath[0], bpath[50]
                        bpath[mvar + 50].y = blueMovePath[mvar].Top;
                    }
                    if (!(redMovePath[mvar] == null))
                    {
                        rpath[mvar + 50].x = redMovePath[mvar].Left; //bpath[0], bpath[50]
                        rpath[mvar + 50].y = redMovePath[mvar].Top;
                    }
                    if (!(greenMovePath[mvar] == null))
                    {
                        gpath[mvar + 50].x = greenMovePath[mvar].Left; //bpath[0], bpath[50]
                        gpath[mvar + 50].y = greenMovePath[mvar].Top;
                    }
                    if (!(yellowMovePath[mvar] == null))
                    {
                        ypath[mvar + 50].x = yellowMovePath[mvar].Left; //bpath[0], bpath[50]
                        ypath[mvar + 50].y = yellowMovePath[mvar].Top;
                    }
                    mvar++;
                }
            }
            bpath[56].x = 310; bpath[56].y = 330;
            rpath[56].x = 250; rpath[56].y = 290;
            gpath[56].x = 290; gpath[56].y = 270;
            ypath[56].x = 350; ypath[56].y = 310;

            for (int jvar = 1; jvar <= 4; jvar++)
            {
                //plates[ivar, jvar] = new Button();
                B[jvar] = (Button)Controls["buttonB" + jvar];
                R[jvar] = (Button)Controls["buttonR" + jvar];
                Y[jvar] = (Button)Controls["buttonY" + jvar];
                G[jvar] = (Button)Controls["buttonG" + jvar];
            }

            R[1].BringToFront();
            B[1].BringToFront();
            Y[1].BringToFront();
            G[1].BringToFront();

            ROLL = initializeRoll();
            btnROLL.BackColor = Color.SkyBlue;

            //ovalShapes initialize
            int os_indexVal;
            for (int ivar = 1; ivar <= 3; ivar++)
            {
                for (int jvar = 1; jvar <= 3; jvar++)
                {
                    //plates[ivar, jvar] = new Button();
                    os_indexVal = shapeContainer1.Shapes.IndexOfKey("ovalShape" + ivar + "" + jvar);
                    OS[((ivar - 1) * 3) + jvar - 1] = (OvalShape)shapeContainer1.Shapes.get_Item(os_indexVal);
                }
            }

            OS[0] = ovalShape11; OS[1] = ovalShape12; OS[2] = ovalShape13;
            OS[3] = ovalShape21; OS[4] = ovalShape22; OS[5] = ovalShape23;
            OS[6] = ovalShape31; OS[7] = ovalShape32; OS[8] = ovalShape33;

            for (int i = 1; i <= 4; i++)
            {
                B[i].Enabled = false;
                //R[i].Enabled = false;
                //Y[i].Enabled = false;
                G[i].Enabled = false;
            }
        }

        private int initializeRoll()
        {
            moveSequence[0] = new int();
            moveSequence[1] = new int();

            if (Configuration.firstMoveColorCode == 1)//Blue
            {
                moveSequence = new int[] { 1, 3 };
                ROLL = 0;
            }
            if (Configuration.firstMoveColorCode == 3)//Green
            {
                moveSequence = new int[] { 3, 1 };
                ROLL = 0;
            }
            return ROLL;
        }

        private void rollNext()
        {
            ROLL = (ROLL + 1) % 2;
        }

        private void btnRoll_Click(object sender, EventArgs e)
        {
            btnROLL.Enabled = false;
            Q = new Random();
            Val = Q.Next(1, 7);
            textBox1.Text = Val.ToString();

            setdice(Val);

            switch (moveSequence[ROLL])
            {
                case 1://blue to move
                    //for (dr = 1; dr <= 4; dr++)
                    //{
                    //    //B[dr].Enabled= true; 
                    //    disableBeadBtn(new Button[][] { G }); //Disable other beads, So, thry can't be moved
                    //    //R[dr].Enabled= false;
                    //    //Y[dr].Enabled = false;
                    //}

                    disableBeadBtn(new Button[][] { G });

                    if (fmove[moveSequence[ROLL]] == 0) //??-neverMovedTillNow
                    {
                        //firstmoveblue(Val);
                        firstMove(1, B, bbead);
                        btnROLL.Enabled = true;
                    }
                    else
                    {
                        if ((numbbeadsopen == 1) && (Val != 6)) // if only one bead open and val!=6 ..AutoMove
                        { moveblue(Val); }
                        else
                        {
                            enableBeadBtns(Val, B, bbead);
                            disableBeadBtn(new Button[][] { G, Y, R });
                            //indicate Blue Should be making a move now
                            indicateBlueToMove();
                        }
                    }
                    break;
                case 3: //green to move
                    //for (dr = 1; dr <= 4; dr++)
                    //{
                    //    B[dr].Enabled= false; 
                    //    //G[dr].Enabled = false;
                    //    //R[dr].Enabled = false;
                    //    //Y[dr].Enabled = false;
                    //}

                    disableBeadBtn(new Button[][] { G });

                    if (fmove[moveSequence[ROLL]] == 0)
                    {
                        firstmovegreen(Val);
                        btnROLL.Enabled = true;

                    }
                    else
                    {
                        if ((numgbeadsopen == 1) && (Val != 6)) // ...Auto Move
                        { movegreen(Val); }
                        else
                        {
                            enableBeadBtns(Val, G, bbead);
                            disableBeadBtn(new Button[][] { B, Y, R });
                            //indicate Green Should be making a move now
                            indicateGreenToMove();
                        }
                    }
                    break;
                case 2: if (fmove[ROLL] == 0)
                    {
                        //firstmoveblue();
                    }
                    else
                    {
                        //moveBlue();
                    }
                    break;
                case 4: if (fmove[ROLL] == 0)
                    {
                        //firstmoveblue();
                    }
                    else
                    {
                        //moveBlue();
                    }
                    break;

            }


        }

        private void disableBeadBtn(Button[][] button)
        {
            foreach (Button[] btnA in button)
            {
                for (int i = 1; i < btnA.Length; i++)
                {
                    btnA[i].Enabled = false;
                }
            }
        }

        private void indicateBlueToMove()
        {
            rectangleShapeBorder.BorderColor = Color.SkyBlue;
            rectangleShapeDice.BackColor = Color.DodgerBlue;

            rectangleShapeGB.BorderColor = Color.ForestGreen;
            rectangleShapeBB.BorderColor = Color.Black;
        }

        private void indicateGreenToMove()
        {
            rectangleShapeBorder.BorderColor = Color.Green;
            rectangleShapeDice.BackColor = Color.ForestGreen;

            rectangleShapeGB.BorderColor = Color.Black;
            rectangleShapeBB.BorderColor = Color.DodgerBlue;
        }

        private void enableBeadBtns(int diceVal, Button[] beadBtns, bead[] bBeadBtns)
        {
            if (diceVal == 6)
            {
                for (int i = 1; i <= 4; i++)
                {
                    beadBtns[i].Enabled = true;
                }
            }
            else
            {
                for (int i = 1; i <= 4; i++)
                {
                    if (bBeadBtns[i].status == 1)
                    {
                        beadBtns[i].Enabled = true;
                    }
                    else
                    {
                        beadBtns[i].Enabled = false;
                    }
                }

            }
        }

        private void setdice(int Val)
        {
            for (osl = 0; osl <= 8; osl++)
            {
                OS[osl].Visible = true;
            }
            switch (Val)
            {
                case 1:
                    //ovalShape11.BackColor = Color.Black;
                    //ovalShape12.BackColor = Color.Black;
                    //ovalShape13.BackColor = Color.Black;
                    //ovalShape21.BackColor = Color.Black;
                    ovalShape22.BackColor = Color.Red;
                    //ovalShape23.BackColor = Color.Black;
                    //ovalShape31.BackColor = Color.Black;
                    //ovalShape32.BackColor = Color.Black;
                    //ovalShape33.BackColor = Color.Black;
                    break;
                case 2:
                    ovalShape11.BackColor = Color.Red;
                    ovalShape12.BackColor = Color.Black;
                    ovalShape13.BackColor = Color.Black;
                    ovalShape21.BackColor = Color.Black;
                    ovalShape22.BackColor = Color.Black;
                    ovalShape23.BackColor = Color.Black;
                    ovalShape31.BackColor = Color.Black;
                    ovalShape32.BackColor = Color.Black;
                    ovalShape33.BackColor = Color.Red;
                    break;
                case 3:
                    ovalShape11.BackColor = Color.Red;
                    ovalShape12.BackColor = Color.Black;
                    ovalShape13.BackColor = Color.Black;
                    ovalShape21.BackColor = Color.Black;
                    ovalShape22.BackColor = Color.Red;
                    ovalShape23.BackColor = Color.Black;
                    ovalShape31.BackColor = Color.Black;
                    ovalShape32.BackColor = Color.Black;
                    ovalShape33.BackColor = Color.Red;
                    break;
                case 4:
                    ovalShape11.BackColor = Color.Red;
                    ovalShape12.BackColor = Color.Black;
                    ovalShape13.BackColor = Color.Red;
                    ovalShape21.BackColor = Color.Black;
                    ovalShape22.BackColor = Color.Black;
                    ovalShape23.BackColor = Color.Black;
                    ovalShape31.BackColor = Color.Red;
                    ovalShape32.BackColor = Color.Black;
                    ovalShape33.BackColor = Color.Red;
                    break;
                case 5:
                    ovalShape11.BackColor = Color.Red;
                    ovalShape12.BackColor = Color.Black;
                    ovalShape13.BackColor = Color.Red;
                    ovalShape21.BackColor = Color.Black;
                    ovalShape22.BackColor = Color.Red;
                    ovalShape23.BackColor = Color.Black;
                    ovalShape31.BackColor = Color.Red;
                    ovalShape32.BackColor = Color.Black;
                    ovalShape33.BackColor = Color.Red;
                    break;
                case 6:
                    ovalShape11.BackColor = Color.Red;
                    ovalShape12.BackColor = Color.Black;
                    ovalShape13.BackColor = Color.Red;
                    ovalShape21.BackColor = Color.Red;
                    ovalShape22.BackColor = Color.Black;
                    ovalShape23.BackColor = Color.Red;
                    ovalShape31.BackColor = Color.Red;
                    ovalShape32.BackColor = Color.Black;
                    ovalShape33.BackColor = Color.Red;
                    break;


            }
            for (osl = 0; osl <= 8; osl++)
            {
                if (OS[osl].BackColor == Color.Black)
                {
                    OS[osl].Visible = false;
                }
            }
        }

        private void resetdice()
        {
            ovalShape11.BackColor = Color.Black;
            ovalShape12.BackColor = Color.Black;
            ovalShape13.BackColor = Color.Black;
            ovalShape21.BackColor = Color.Black;
            ovalShape22.BackColor = Color.Black;
            ovalShape23.BackColor = Color.Black;
            ovalShape31.BackColor = Color.Black;
            ovalShape32.BackColor = Color.Black;
            ovalShape33.BackColor = Color.Black;
        }

        private void moveblue(int Val)
        {
            if (bbead[1].status == 1)
            {

                if ((bbead[1].loc + Val) == 55)
                {
                    if ((Val == 6) || (Val == 1))
                    {
                        bbead[1].loc = bbead[1].loc + 1;
                        B[1].Left = bpath[bbead[1].loc].x;
                        B[1].Top = bpath[bbead[1].loc].y;
                        ROLL = ROLL + 1;
                    }
                }
                else if (((bbead[1].loc + Val) <= 56) && (Val != 6))
                {
                    bbead[1].loc = bbead[1].loc + Val;
                    B[1].Left = bpath[bbead[1].loc].x;
                    B[1].Top = bpath[bbead[1].loc].y;
                    ROLL = ROLL + 1;
                }
                else if (((bbead[1].loc + Val) <= 56) && (Val == 6))
                {
                    bbead[1].loc = bbead[1].loc + Val;
                    B[1].Left = bpath[bbead[1].loc].x;
                    B[1].Top = bpath[bbead[1].loc].y;

                }
                else if (((bbead[1].loc + Val) > 56))
                {
                    ROLL = 2;
                }
            }
            updateroll();
            unlock();
            if (Val != 6)
            {
                B[1].Enabled = false; B[2].Enabled = false; B[3].Enabled = false; B[4].Enabled = false;
            }
        }

        private void movegreen(int Val)
        {
            if (gbead[1].status == 1)
                if ((gbead[1].loc + Val) == 55)
                {
                    if ((Val == 6) || (Val == 1))
                    {
                        gbead[1].loc = gbead[1].loc + 1;
                        G[1].Left = gpath[gbead[1].loc].x;
                        G[1].Top = gpath[gbead[1].loc].y;
                        ROLL = ROLL + 1;
                    }
                }
                else if (((gbead[1].loc + Val) <= 56) && (Val != 6))
                {
                    gbead[1].loc = gbead[1].loc + Val;
                    G[1].Left = gpath[gbead[1].loc].x;
                    G[1].Top = gpath[gbead[1].loc].y;
                    ROLL = 1;
                }
                else if (((gbead[1].loc + Val) <= 56) && (Val == 6))
                {
                    gbead[1].loc = gbead[1].loc + Val;
                    G[1].Left = gpath[gbead[1].loc].x;
                    G[1].Top = gpath[gbead[1].loc].y;

                }
                else if (((gbead[1].loc + Val) > 56))
                {
                    ROLL = 1;
                }
            updateroll();
            unlock();
            if (Val != 6)
            {
                G[1].Enabled = false; G[2].Enabled = false; G[3].Enabled = false; G[4].Enabled = false;
            }
        }

        private void firstmovegreen(int Val)
        {
            if ((Val == 6) || (Val == 1))
            {
                fmove[ROLL] = 1;
                if (gbead[1].status == 0)
                {
                    gbead[1].status = 1;
                    gbead[1].loc = 0;
                    G[1].Left = gpath[gbead[1].loc].x;
                    G[1].Top = gpath[gbead[1].loc].y;
                    G[1].Enabled = true;
                    numgbeadsopen = numgbeadsopen + 1;
                }
            }
            else
            {
                ROLL = 1;
                updateroll();

            }

        }

        //reflect who should be making the move now
        private void updateroll()
        {
            rollNext();

            if (moveSequence[ROLL] == 3)
            {
                textBox2.Text = "Green";
                btnROLL.BackColor = Color.Green;
                //rectangleShapeBorder.BorderColor = Color.Green;
                //rectangleShapeGB.BorderColor = Color.Black;
                //rectangleShapeBB.BorderColor = Color.DodgerBlue;
                //rectangleShapeDice.BackColor = Color.ForestGreen;


            }
            else if (moveSequence[ROLL] == 1) //BLUE
            {
                textBox2.Text = "Blue";
                btnROLL.BackColor = Color.SkyBlue;
                //rectangleShapeBorder.BorderColor = Color.SkyBlue;
                //rectangleShapeGB.BorderColor = Color.ForestGreen;
                //rectangleShapeBB.BorderColor = Color.Black;
                //rectangleShapeDice.BackColor = Color.DodgerBlue;


            }
        }

        public void unlock()
        {
            btnROLL.Enabled = true;
            for (dr = 1; dr < 5; dr++)
            {
                B[dr].Enabled = true;
                G[dr].Enabled = true;
                //R[dr].Enabled= false;
                //Y[dr].Enabled = false;  
            }
        }

        private void firstmoveblue(int Val) //bbead, B
        {
            if ((Val == 6) || (Val == 1)) //Can open first bead
            {
                fmove[moveSequence[ROLL]] = 1;
                if (bbead[1].status == 0)
                {
                    bbead[1].status = 1;
                    bbead[1].loc = 0;
                    B[1].Left = bpath[bbead[1].loc].x;
                    B[1].Top = bpath[bbead[1].loc].y;
                    B[1].Enabled = true;
                    numbbeadsopen = numbbeadsopen + 1;
                }
            }
            else
            {
                ROLL = 2;
                updateroll();
            }

        }

        private void firstMove(int colourCode, Button[] beadBtns, bead[] bBeadBtns) // to replace firstMove<Color>
        {
            if ((Val == 6) || (Val == 1)) //Can open first bead
            {
                fmove[moveSequence[ROLL]] = 1;
                if (bBeadBtns[1].status == 0)
                {
                    bBeadBtns[1].status = 1;
                    bBeadBtns[1].loc = 0;
                    beadBtns[1].Left = bpath[bBeadBtns[1].loc].x;
                    beadBtns[1].Top = bpath[bBeadBtns[1].loc].y;
                    beadBtns[1].Enabled = true;
                    numbbeadsopen = numbbeadsopen + 1; //TODO
                }
            }
            else
            {
                //TODO next player should move
                //ROLL = 2;
                //updateroll();
            }
        }

        /**
         * Method 1 - For common button Click Methods for the Grid of Buttons
         * This returns the Number value of button eg: 23 for button23
         **/
        public int getButtonNumber(object sender)
        {
            Button b = sender as Button;
            String s = b.Name;
            s = s.Replace("button", "");
            return Convert.ToInt32(s);
        }

        /**
         * Method 2 - For common button Click Methods for the Grid of Buttons
         * This is the click middleware- Click of button executes this,
         * which calls the generic Click method, providing Number value
         * of the button which was clicked by using Method 1.
         **/
        private void buttonAB_Click(object sender, EventArgs e)
        {
            button_Click(getButtonNumber(sender));
        }

        /**
         * Method 3 - For common button Click Methods for the Grid of Buttons
         * This is the actual code that shuld be executed when any of the 
         * button from the Grid of Button is clicked.
         **/
        private void button_Click(int p)
        {
            {
                performAction(p / 10, p % 10);
            }
        }

        private void performAction(int colourCode, int beadId)
        {
            if (colourCode == 1) //Blue
            {
                if (bbead[beadId].status == 1)
                {
                    if ((bbead[beadId].loc + Val) == 55)
                    {
                        if ((Val == 6) || (Val == 1))
                        {
                            bbead[beadId].loc = bbead[beadId].loc + 1;
                            B[beadId].Left = bpath[bbead[beadId].loc].x;
                            B[beadId].Top = bpath[bbead[beadId].loc].y;
                            ROLL = ROLL + 1;
                        }
                    }
                    else if (((bbead[beadId].loc + Val) <= 56) && (Val != 6))
                    {
                        bbead[beadId].loc = bbead[beadId].loc + Val;
                        B[beadId].Left = bpath[bbead[beadId].loc].x;
                        B[beadId].Top = bpath[bbead[beadId].loc].y;
                        ROLL = ROLL + 1;
                    }
                    else if (((bbead[beadId].loc + Val) <= 56) && (Val == 6))
                    {
                        bbead[beadId].loc = bbead[beadId].loc + Val;
                        B[beadId].Left = bpath[bbead[beadId].loc].x;
                        B[beadId].Top = bpath[bbead[beadId].loc].y;

                    }
                    else if (((bbead[beadId].loc + Val) > 56))
                    {
                        ROLL = 2;
                    }

                    //User Has Moved
                    //if Val was 6, then user has to roll dice again 
                    //else change ROLL value to indicate the next player in line to Move
                    if (Val == 6)
                    {
                        //doNothing, user will rerolldice
                        disableBeadBtn(new Button[][] { B });
                    }
                    else
                    {
                        disableBeadBtn(new Button[][] { B });
                        updateroll();
                    }
                }
                //updateroll();
                unlock();
            }
            else if (colourCode == 3) //Green
            {
                if (gbead[beadId].status == 1)
                {
                    if ((gbead[beadId].loc + Val) == 55)
                    {
                        if ((Val == 6) || (Val == 1))
                        {
                            gbead[beadId].loc = gbead[beadId].loc + 1;
                            G[beadId].Left = gpath[gbead[beadId].loc].x;
                            G[beadId].Top = gpath[gbead[beadId].loc].y;
                            ROLL = ROLL + 1;
                        }
                    }
                    else if (((gbead[beadId].loc + Val) <= 56) && (Val != 6))
                    {
                        gbead[beadId].loc = gbead[1].loc + Val;
                        G[beadId].Left = gpath[gbead[beadId].loc].x;
                        G[beadId].Top = gpath[gbead[beadId].loc].y;
                        ROLL = 1;
                    }
                    else if (((gbead[beadId].loc + Val) <= 56) && (Val == 6))
                    {
                        gbead[beadId].loc = gbead[beadId].loc + Val;
                        G[beadId].Left = gpath[gbead[beadId].loc].x;
                        G[beadId].Top = gpath[gbead[beadId].loc].y;

                    }
                    else if (((gbead[beadId].loc + Val) > 56))
                    {
                        ROLL = 1;
                    }

                    //User Has Moved
                    //if Val was 6, then user has to roll dice again 
                    //else change ROLL value to indicate the next player in line to Move
                    if (Val == 6)
                    {
                        //doNothing, user will rerolldice
                        disableBeadBtn(new Button[][] { G });
                    }
                    else
                    {
                        disableBeadBtn(new Button[][] { G });
                        updateroll();
                    }
                }
                unlock();

            }
        }

        class path
        {
            public int x;
            public int y;

            public path()
            {
                x = new int();
                x = 0;
                y = new int();
                y = 0;
            }
        }

        class bead
        {
            public int loc;
            public int status;

            public bead()
            {
                loc = -5;
                status = 0;
            }
        }
    }

    public class Configuration
    {
        public static int firstMoveColorCode = 1;
        int idColors; //B-1, R-2,G-3,Y-4;

    }
}
