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
        int mode = 0;// 2 Player mode //TODO Recheck
        int[] moveSequence = new int[Configuration.currentPlayerCountConfig];

        //path[] rpath = new path[57]; path[] bpath = new path[57]; path[] gpath = new path[57]; path[] ypath = new path[57];    //path[] apath = new path[57];
        path[][] pathVars;

        //Button[] R = new Button[5];     Button[] B = new Button[5];     Button[] Y = new Button[5];     Button[] G = new Button[5];
        Button[][] BeadVars;

        //bead[] bbead = new bead[5]; bead[] gbead = new bead[5];
        bead[][] beadVars;

        Microsoft.VisualBasic.PowerPacks.OvalShape[] OS = new Microsoft.VisualBasic.PowerPacks.OvalShape[9];

        int[] fmove = new int[5];

        Random Q;

        int Val = 0, ROLL = -1, dr = 1,osl = 0;

        //int numbbeadsopen = 0, numgbeadsopen = 0, numrbeadsopen = 0, numybeadsopen = 0;
        int[] numbeadsopen;

        RectangleShape[] movePath = new RectangleShape[53];

        //RectangleShape[] redMovePath = new RectangleShape[6]; RectangleShape[] greenMovePath = new RectangleShape[6]; RectangleShape[] yellowMovePath = new RectangleShape[6]; RectangleShape[] blueMovePath = new RectangleShape[6];
        RectangleShape[][] colorSpMovePath = new RectangleShape[4][];

        int[] map = new int[Configuration.currentPlayerCountConfig + 1];

        int[] diffs = new int[] { 0,0,27,14,40};

        public ludoLeagueForm()
        {
            InitializeComponent();

            //initialize Color Paths.
            for(int ival = 1; ival <= (Configuration.currentPlayerCountConfig); ival++)
            //foreach (int ival in Configuration.playerColorConfigs[Configuration.currentPlayerCountConfig])
            {
                map[ival] = new int();
                map[ival] = Configuration.playerColorConfigs[Configuration.currentPlayerCountConfig][ival];

                //map[1] //First Color
                //map[2] //SecondColor
                //switch (map[1])
            }

            //path[][] pathVars;
            pathVars = new path[Configuration.currentPlayerCountConfig + 1][];

            //Button[][] Bead;
            BeadVars = new Button[Configuration.currentPlayerCountConfig + 1][];


            //bead[][] beadVars;
            beadVars = new bead[Configuration.currentPlayerCountConfig + 1][];

            //int[] numbeadopen;
            numbeadsopen = new int[Configuration.currentPlayerCountConfig + 1];

            colorSpMovePath = new RectangleShape[Configuration.currentPlayerCountConfig + 1][];
            for (int ival = 1; ival <= (Configuration.currentPlayerCountConfig); ival++)
            {
                colorSpMovePath[ival] = new RectangleShape[6]; //which Color = map[ivar];
                beadVars[ival] = new bead[5];
                BeadVars[ival] = new Button[5];
                pathVars[ival] = new path[57];
            }


            for (int ivar = 1; ivar <= Configuration.currentPlayerCountConfig; ivar++)
            {
                for (int jvar = 1; jvar <= 56; jvar++)
                {
                    pathVars[ivar][jvar] = new path();
                }
            }
            for (int ivar = 1; ivar <= Configuration.currentPlayerCountConfig; ivar++)
            {
                OS[ivar - 1] = new Microsoft.VisualBasic.PowerPacks.OvalShape();
                OS[4 + ivar - 1] = new Microsoft.VisualBasic.PowerPacks.OvalShape();

                if (Configuration.currentPlayerCountConfig == 4)
                {
                    if (ivar >= 3)
                    {
                        continue;
                    }
                }
                for (int jvar = 1; jvar <= 4; jvar++)
                {
                    BeadVars[ivar][jvar] = new Button();
                    BeadVars[ivar][jvar] = (Button)Controls["button" + ivar + "" + jvar];
                    beadVars[ivar][jvar] = new bead();
                    colorSpMovePath[ivar][jvar] = new RectangleShape();
                }
                numbeadsopen[ivar] = new int();
                numbeadsopen[ivar] = 0;
                colorSpMovePath[ivar][5] = new RectangleShape();
                fmove[ivar] = 0;
            }
            OS[8] = new Microsoft.VisualBasic.PowerPacks.OvalShape();
        }

        private void ludoLeagueForm_Load(object sender, EventArgs e)
        {
            //R[1].BringToFront();
            rectangleShapeDice.SendToBack();

            for (int mvar = 1; mvar <= 52;)
            {
                movePath[mvar] = new RectangleShape();
                int rs_index = shapeContainer1.Shapes.IndexOfKey("rectangleShape" + mvar);
                if (rs_index != -1)
                {
                    movePath[mvar] = (RectangleShape)shapeContainer1.Shapes.get_Item(rs_index);
                    if(!(movePath[mvar] == null))
                    {
                        if ((Configuration.currentPlayerCountConfig == 2) || (Configuration.currentPlayerCountConfig == 4))
                        {
                            pathVars[1][mvar].x = movePath[mvar].Left;// - 1
                            pathVars[1][mvar].y = movePath[mvar].Top; // - 1																																
                        }

                        for (int ivar = 2; ivar <= 4;ivar++ )
                        {
                            if (ivar == 3 || ivar == 4)
                            {
                                if ((Configuration.currentPlayerCountConfig == 2))
                                {
                                    continue;
                                }
                            }
                            if (mvar < diffs[ivar])
                            {
                                //if ((Configuration.currentPlayerCountConfig == 4))
                                //{
                                    pathVars[ivar][mvar + (52 - diffs[ivar])].x = movePath[mvar].Left;
                                    pathVars[ivar][mvar + (52 - diffs[ivar])].y = movePath[mvar].Top;
                                //}
                            }
                            else
                            {
                                //if ((Configuration.currentPlayerCountConfig == 4))
                                //{
                                    pathVars[ivar][mvar - diffs[ivar] + 1].x = movePath[mvar].Left;
                                    pathVars[ivar][mvar - diffs[ivar] + 1].y = movePath[mvar].Top;
                                //}
                            }
                        }
                    }
                }
                mvar++;
            }
            
            for (int mvar = 1; mvar <= 5; )
            {
                if (true) //rs_indexB != -1
                {
                    for (int ivar = 1; ivar <= 4;ivar++ )
                    {
                        if ((Configuration.currentPlayerCountConfig == 2))
                        {
                            if (ivar == 3 || ivar == 4)
                            {
                                continue;
                            }
                        }

                        int rs_index = shapeContainer1.Shapes.IndexOfKey("rectangleShapeH" + ivar + "" + mvar);
                        if (rs_index != -1) //rs_indexB != -1
                        {
                            colorSpMovePath[ivar][mvar] = (RectangleShape)shapeContainer1.Shapes.get_Item(rs_index);

                            pathVars[ivar][mvar + 50].x = colorSpMovePath[ivar][mvar].Left;
                            pathVars[ivar][mvar + 50].y = colorSpMovePath[ivar][mvar].Top;
                        }                       
                    }
                    mvar++;
                }
            }

            pathVars[1][56].x = 310; pathVars[1][56].y = 330;
            pathVars[2][56].x = 250; pathVars[2][56].y = 290;
            if ((Configuration.currentPlayerCountConfig == 4))
            {
                pathVars[3][56].x = 290; pathVars[3][56].y = 270;
                pathVars[4][56].x = 350; pathVars[4][56].y = 310;
            }

            for (int jvar = 1; jvar <= Configuration.currentPlayerCountConfig; jvar++)
            {
                BeadVars[jvar][1].BringToFront(); //TODO
            }

            ROLL = initializeRoll();
            btnROLL.BackColor = Color.SkyBlue; //TODO

            //ovalShapes initialize
            int os_indexVal;
            for (int ivar = 1; ivar <= 3; ivar++)
            {
                for (int jvar = 1; jvar <= 3; jvar++)
                {
                    //plates[ivar, jvar] = new Button();
                    os_indexVal = shapeContainer1.Shapes.IndexOfKey("ovalShape"+ ivar+ "" + jvar);
                    OS[((ivar-1)*3) + jvar - 1] = (OvalShape)shapeContainer1.Shapes.get_Item(os_indexVal);
                }
            }
        }

        private int initializeRoll()
        {
            moveSequence[0] = new int();
            moveSequence[1] = new int();

            if (Configuration.firstMoveColorCode == 1)//Blue
            {
                moveSequence = new int[] { 1, 3};
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
            ROLL = (ROLL + 1) % 2;//TODO
            textBox3.Text += "ROLL - " + ROLL + "\r\n";
            
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
                    textBox3.Text += "Blue to move, Val - " + Val + ",";
                    disableBeadBtn(BeadVars[ROLL + 1]); //disable except this one
                    if (fmove[moveSequence[ROLL]] == 0) //??-neverMovedTillNow
                        {
                            //textBox3.Text += "Blue 1st move";
                            //firstmoveblue(Val);
                            firstMove(1, BeadVars[ROLL + 1], beadVars[ROLL + 1]);
                            btnROLL.Enabled = true;
                        }
                        else
                        {
                            if ((numbeadsopen[ROLL + 1] == 1) && (Val != 6)) // if only one bead open and val!=6 ..AutoMove
                            { moveblue(Val); }
                            else
                            {
                                enableBeadBtns(Val, BeadVars[ROLL + 1], beadVars[ROLL + 1]);
                                disableBeadBtn(BeadVars[ROLL + 1]);
                                //indicate Blue Should be making a move now
                                indicateNextColorToMove();
                            }
                        }
                    break;
                case 3: //green to move
                    textBox3.Text += "Green to move, Val - " + Val + ",";
                    disableBeadBtn(BeadVars[ROLL + 1]);
                    if (fmove[moveSequence[ROLL]] == 0)
                    {
                        //textBox3.Text += "Green 1st move";
                        firstMove(3, BeadVars[ROLL + 1], beadVars[ROLL + 1]);
                        btnROLL.Enabled = true;

                    }
                    else
                    {
                        if ((numbeadsopen[ROLL + 1] == 1) && (Val != 6)) // ...Auto Move
                        { movegreen(Val); }
                        else
                        {
                            enableBeadBtns(Val, BeadVars[ROLL + 1], beadVars[ROLL + 1]);
                            disableBeadBtn(BeadVars[ROLL + 1]);
                            //indicate Green Should be making a move now
                            indicateNextColorToMove();
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

        private void disableBeadBtn(Button[] button)
        {
            foreach(Button[] btnA in BeadVars)
            {
                if (btnA != button && (btnA !=null))
                {
                    for (int i = 1; i < btnA.Length; i++)
                    {
                        btnA[i].Enabled = false;
                    }
                }
            }
        }

        private void indicateNextColorToMove()
        {
            switch (ROLL)
            {
                case 1: rectangleShapeBorder.BorderColor = Color.SkyBlue;
                    rectangleShapeDice.BackColor = Color.DodgerBlue;

                    rectangleShapeGB.BorderColor = Color.ForestGreen;
                    rectangleShapeBB.BorderColor = Color.Black;
                    break;
                case 3: 
                    rectangleShapeBorder.BorderColor = Color.Green;
                    rectangleShapeDice.BackColor = Color.ForestGreen;

                    rectangleShapeGB.BorderColor = Color.Black;
                    rectangleShapeBB.BorderColor = Color.DodgerBlue;
                    break;
            }
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
                OS[osl].BackColor = Color.Red;
            }
            switch (Val){
                case 1:
                    ovalShape11.Visible = false;
                    ovalShape12.Visible = false;
                    ovalShape13.Visible = false;
                    ovalShape21.Visible = false;
                    ovalShape22.Visible = true;
                    ovalShape23.Visible = false;
                    ovalShape31.Visible = false;
                    ovalShape32.Visible = false;
                    ovalShape33.Visible = false;
                    break;
                case 2:
                    ovalShape11.Visible = true;
                    ovalShape12.Visible = false;
                    ovalShape13.Visible = false;
                    ovalShape21.Visible = false;
                    ovalShape22.Visible = false;
                    ovalShape23.Visible = false;
                    ovalShape31.Visible = false;
                    ovalShape32.Visible = false;
                    ovalShape33.Visible = true;
                    break;
                case 3:
                    ovalShape11.Visible = true;
                    ovalShape12.Visible = false;
                    ovalShape13.Visible = false;
                    ovalShape21.Visible = false;
                    ovalShape22.Visible = true;
                    ovalShape23.Visible = false;
                    ovalShape31.Visible = false;
                    ovalShape32.Visible = false;
                    ovalShape33.Visible = true;
                    break;
                case 4:
                    ovalShape11.Visible = true;
                    ovalShape12.Visible = false;
                    ovalShape13.Visible = true;
                    ovalShape21.Visible = false;
                    ovalShape22.Visible = false;
                    ovalShape23.Visible = false;
                    ovalShape31.Visible = true;
                    ovalShape32.Visible = false;
                    ovalShape33.Visible = true;
                    break;
                case 5:
                    ovalShape11.Visible = true;
                    ovalShape12.Visible = false;
                    ovalShape13.Visible = true;
                    ovalShape21.Visible = false;
                    ovalShape22.Visible = true;
                    ovalShape23.Visible = false;
                    ovalShape31.Visible = true;
                    ovalShape32.Visible = false;
                    ovalShape33.Visible = true;
                    break;
                case 6:
                    ovalShape11.Visible = true;
                    ovalShape12.Visible = false;
                    ovalShape13.Visible = true;
                    ovalShape21.Visible = true;
                    ovalShape22.Visible = false;
                    ovalShape23.Visible = true;
                    ovalShape31.Visible = true;
                    ovalShape32.Visible = false;
                    ovalShape33.Visible = true;
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
            textBox3.Text += "auto-move blue";
            if (beadVars[ROLL + 1][1].status == 1) //beadVars[ROLL + 1]
            {

                if ((beadVars[ROLL + 1][1].loc + Val) == 55)
                {
                    if ((Val == 6) || (Val == 1))
                    {
                        beadVars[ROLL + 1][1].loc = beadVars[ROLL + 1][1].loc + 1;
                        BeadVars[ROLL + 1][1].Left = pathVars[ROLL + 1][beadVars[ROLL + 1][1].loc].x; //pathVars[ROLL + 1]
                        BeadVars[ROLL + 1][1].Top = pathVars[ROLL + 1][beadVars[ROLL + 1][1].loc].y;
                        //ROLL = ROLL + 1;
                    }
                }
                else if (((beadVars[ROLL + 1][1].loc + Val) <= 56) && (Val != 6))
                {
                    beadVars[ROLL + 1][1].loc = beadVars[ROLL + 1][1].loc + Val;
                    BeadVars[ROLL + 1][1].Left = pathVars[ROLL + 1][beadVars[ROLL + 1][1].loc].x;
                    BeadVars[ROLL + 1][1].Top = pathVars[ROLL + 1][beadVars[ROLL + 1][1].loc].y;
                    //ROLL = ROLL + 1;
                }
                else if (((beadVars[ROLL + 1][1].loc + Val) <= 56) && (Val == 6))
                {
                    beadVars[ROLL + 1][1].loc = beadVars[ROLL + 1][1].loc + Val;
                    BeadVars[ROLL + 1][1].Left = pathVars[ROLL + 1][beadVars[ROLL + 1][1].loc].x;
                    BeadVars[ROLL + 1][1].Top = pathVars[ROLL + 1][beadVars[ROLL + 1][1].loc].y;

                }
                else if (((beadVars[ROLL + 1][1].loc + Val) > 56))
                {
                    //ROLL = 2;
                }
            }
            updateroll();
            unlock();
            if (Val != 6)
            {
                BeadVars[ROLL + 1][1].Enabled = false; BeadVars[ROLL + 1][2].Enabled = false; BeadVars[ROLL + 1][3].Enabled = false; BeadVars[ROLL + 1][4].Enabled = false;
            }
        }

        private void movegreen(int Val)
        {
            textBox3.Text += "auto-move green";
            if (beadVars[ROLL + 1][1].status == 1)
                if ((beadVars[ROLL + 1][1].loc + Val) == 55)
                {
                    if ((Val == 6) || (Val == 1))
                    {
                        beadVars[ROLL + 1][1].loc = beadVars[ROLL + 1][1].loc + 1;
                        BeadVars[ROLL + 1][1].Left = pathVars[ROLL + 1][beadVars[ROLL + 1][1].loc].x; //beadVars[ROLL + 1]
                        BeadVars[ROLL + 1][1].Top = pathVars[ROLL + 1][beadVars[ROLL + 1][1].loc].y;
                        //ROLL = ROLL + 1;
                    }
                }
                else if (((beadVars[ROLL + 1][1].loc + Val) <= 56) && (Val != 6))
                {
                    beadVars[ROLL + 1][1].loc = beadVars[ROLL + 1][1].loc + Val;
                    BeadVars[ROLL + 1][1].Left = pathVars[ROLL + 1][beadVars[ROLL + 1][1].loc].x;
                    BeadVars[ROLL + 1][1].Top = pathVars[ROLL + 1][beadVars[ROLL + 1][1].loc].y;
                    //ROLL = 1;
                }
                else if (((beadVars[ROLL + 1][1].loc + Val) <= 56) && (Val == 6))
                {
                    beadVars[ROLL + 1][1].loc = beadVars[ROLL + 1][1].loc + Val;
                    BeadVars[ROLL + 1][1].Left = pathVars[ROLL + 1][beadVars[ROLL + 1][1].loc].x;
                    BeadVars[ROLL + 1][1].Top = pathVars[ROLL + 1][beadVars[ROLL + 1][1].loc].y;

                }
                else if (((beadVars[ROLL + 1][1].loc + Val) > 56))
                {
                    //ROLL = 1;
                }
            updateroll();
            unlock();
            if (Val != 6)
            {
                BeadVars[ROLL + 1][1].Enabled = false; BeadVars[ROLL + 1][2].Enabled = false; BeadVars[ROLL + 1][3].Enabled = false; BeadVars[ROLL + 1][4].Enabled = false;
            }
        }

        //reflect who should be making the move now
        private void updateroll()
        {
            rollNext();
            textBox3.Text += "ms : " + moveSequence[ROLL] + ",";
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
                BeadVars[ROLL + 1][dr].Enabled = true;
                BeadVars[ROLL + 1][dr].Enabled = true;
                //R[dr].Enabled= false;
                //Y[dr].Enabled = false;  
            }
        }

        private void firstMove(int colourCode, Button[] beadBtns, bead[] BeadBtns) // to replace firstMove<Color>
        {
            if ((Val == 6) || (Val == 1)) //Can open first bead
            {
                textBox3.Text += "1st move";
                fmove[moveSequence[ROLL]] = 1;
                if (BeadBtns[1].status == 0)
                {
                    BeadBtns[1].status = 1;
                    BeadBtns[1].loc = 1;
                    beadBtns[1].Left = pathVars[ROLL + 1][BeadBtns[1].loc].x;
                    beadBtns[1].Top = pathVars[ROLL + 1][BeadBtns[1].loc].y;
                    beadBtns[1].Enabled = true;
                    numbeadsopen[ROLL + 1] = numbeadsopen[ROLL + 1] + 1; //TODO
                }
            }
            else
            {
                //TODO next player should move
                //rollNext();
                updateroll();
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
                if ((beadVars[ROLL + 1][beadId].status == 1)) //beadVars[ROLL + 1][beadId]
                {
                    if ((beadVars[ROLL + 1][beadId].loc + Val) == 55)
                    {
                        if ((Val == 6) || (Val == 1))
                        {
                            beadVars[ROLL + 1][beadId].loc = beadVars[ROLL + 1][beadId].loc + 1;
                            BeadVars[ROLL + 1][beadId].Left = pathVars[ROLL + 1][beadVars[ROLL + 1][beadId].loc].x;
                            BeadVars[ROLL + 1][beadId].Top = pathVars[ROLL + 1][beadVars[ROLL + 1][beadId].loc].y;
                            //ROLL = ROLL + 1;
                            updateroll(); unlock();
                        }
                    }
                    else if (((beadVars[ROLL + 1][beadId].loc + Val) <= 56) && (Val != 6))
                    {
                        beadVars[ROLL + 1][beadId].loc = beadVars[ROLL + 1][1].loc + Val;
                        BeadVars[ROLL + 1][beadId].Left = pathVars[ROLL + 1][beadVars[ROLL + 1][beadId].loc].x;
                        BeadVars[ROLL + 1][beadId].Top = pathVars[ROLL + 1][beadVars[ROLL + 1][beadId].loc].y;
                        //ROLL = ROLL + 1;
                        updateroll(); unlock();
                    }
                    else if (((beadVars[ROLL + 1][beadId].loc + Val) <= 56) && (Val == 6))
                    {
                        beadVars[ROLL + 1][beadId].loc = beadVars[ROLL + 1][beadId].loc + Val;
                        BeadVars[ROLL + 1][beadId].Left = pathVars[ROLL + 1][beadVars[ROLL + 1][beadId].loc].x;
                        BeadVars[ROLL + 1][beadId].Top = pathVars[ROLL + 1][beadVars[ROLL + 1][beadId].loc].y;
                        updateroll(); unlock();
                    }
                    else if (((beadVars[ROLL + 1][beadId].loc + Val) > 56))
                    {
                        //ROLL = 2;
                    }

                    //User Has Moved
                    //if Val was 6, then user has to roll dice again 
                    //else change ROLL value to indicate the next player in line to Move
                    if (Val == 6)
                    {
                        //doNothing, user will rerolldice
                        disableBeadBtn(BeadVars[ROLL + 1]);
                    }
                    else
                    {
                        disableBeadBtn(BeadVars[ROLL + 1]);
                        updateroll(); unlock();
                    }
                }
                else if (((beadVars[ROLL + 1][beadId].status == 0) && (beadVars[ROLL + 1][beadId].loc == -5)))
                {
                    if ((Val == 6) || (Val == 1)) //Can open first bead
                    {
                        textBox3.Text += "1st move";
                        fmove[moveSequence[ROLL]] = 1;
                        if (beadVars[ROLL + 1][beadId].status == 0)
                        {
                            beadVars[ROLL + 1][beadId].status = 1;
                            beadVars[ROLL + 1][beadId].loc = 1;
                            BeadVars[ROLL + 1][beadId].Left = pathVars[ROLL + 1][beadVars[ROLL + 1][beadId].loc].x;
                            BeadVars[ROLL + 1][beadId].Top = pathVars[ROLL + 1][beadVars[ROLL + 1][beadId].loc].y;
                            BeadVars[ROLL + 1][beadId].Enabled = true;
                            numbeadsopen[ROLL + 1] = numbeadsopen[ROLL + 1] + 1; //TODO
                        }
                        //updateroll();
                        unlock();
                    }
                    else
                    {
                        //Nothing should happen on click
                    }
                }
            }
            else if (colourCode == 3) //Green
            {
                if ((beadVars[ROLL + 1][beadId].status == 1)) //beadVars[ROLL + 1][beadId]
                {
                    if ((beadVars[ROLL + 1][beadId].loc + Val) == 55)
                    {
                        if ((Val == 6) || (Val == 1))
                        {
                            beadVars[ROLL + 1][beadId].loc = beadVars[ROLL + 1][beadId].loc + 1;
                            BeadVars[ROLL + 1][beadId].Left = pathVars[ROLL + 1][beadVars[ROLL + 1][beadId].loc].x;
                            BeadVars[ROLL + 1][beadId].Top = pathVars[ROLL + 1][beadVars[ROLL + 1][beadId].loc].y;
                            //ROLL = ROLL + 1;
                            updateroll(); unlock();
                        }
                    }
                    else if (((beadVars[ROLL + 1][beadId].loc + Val) <= 56) && (Val != 6))
                    {
                        beadVars[ROLL + 1][beadId].loc = beadVars[ROLL + 1][1].loc + Val;
                        BeadVars[ROLL + 1][beadId].Left = pathVars[ROLL + 1][beadVars[ROLL + 1][beadId].loc].x;
                        BeadVars[ROLL + 1][beadId].Top = pathVars[ROLL + 1][beadVars[ROLL + 1][beadId].loc].y;
                        //ROLL = ROLL + 1;
                        updateroll(); unlock();
                    }
                    else if (((beadVars[ROLL + 1][beadId].loc + Val) <= 56) && (Val == 6))
                    {
                        beadVars[ROLL + 1][beadId].loc = beadVars[ROLL + 1][beadId].loc + Val;
                        BeadVars[ROLL + 1][beadId].Left = pathVars[ROLL + 1][beadVars[ROLL + 1][beadId].loc].x;
                        BeadVars[ROLL + 1][beadId].Top = pathVars[ROLL + 1][beadVars[ROLL + 1][beadId].loc].y;
                        updateroll(); unlock();
                    }
                    else if (((beadVars[ROLL + 1][beadId].loc + Val) > 56))
                    {
                        //ROLL = 2;
                    }

                    //User Has Moved
                    //if Val was 6, then user has to roll dice again 
                    //else change ROLL value to indicate the next player in line to Move
                    if (Val == 6)
                    {
                        //doNothing, user will rerolldice
                        disableBeadBtn(BeadVars[ROLL + 1]);
                    }
                    else
                    {
                        disableBeadBtn(BeadVars[ROLL + 1]);
                        updateroll(); unlock();
                    }
                }
                else if (((beadVars[ROLL + 1][beadId].status == 0) && (beadVars[ROLL + 1][beadId].loc == -5)))
                {
                    if ((Val == 6) || (Val == 1)) //Can open first bead
                    {
                        textBox3.Text += "1st move";
                        fmove[moveSequence[ROLL]] = 1;
                        if (beadVars[ROLL + 1][beadId].status == 0)
                        {
                            beadVars[ROLL + 1][beadId].status = 1;
                            beadVars[ROLL + 1][beadId].loc = 1;
                            BeadVars[ROLL + 1][beadId].Left = pathVars[ROLL + 1][beadVars[ROLL + 1][beadId].loc].x;
                            BeadVars[ROLL + 1][beadId].Top = pathVars[ROLL + 1][beadVars[ROLL + 1][beadId].loc].y;
                            BeadVars[ROLL + 1][beadId].Enabled = true;
                            numbeadsopen[ROLL + 1] = numbeadsopen[ROLL + 1] + 1; //TODO
                        }
                        //updateroll();
                        unlock();
                    }
                    else
                    {
                        //Nothing should happen on click
                    }
                }
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

    public class Constants
    {
        public static int ZERO = 0;
        public static int BLUE = 1;
        public static int RED = 2;
        public static int GREEN = 3;
        public static int YELLOW = 4;
    }

    public class Configuration
    {
        public static int firstMoveColorCode = 1;
        public static int currentPlayerCountConfig = 2;
        public static int[][] playerColorConfigs = 
            new int[][] { null,
                          null,
                          new int[] { Constants.ZERO, Constants.BLUE, Constants.GREEN },
                          null,
                          new int[] { Constants.ZERO, Constants.RED, Constants.GREEN, Constants.YELLOW, Constants.BLUE}
            };

    }
}
