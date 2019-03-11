using System;

namespace battleship_dotnet
{
    class Board
    {
        private uint height;
        private uint width;
        private int[,] board_state;
        private uint num_of_ship_blocks = 0;

        public Board() {
            this.height = 0;
            this.width = 0;
            this.board_state = new int[this.height, this.width];
            Console.WriteLine("Board created with height = {0}, width = {1}.", this.height, this.width);
            Console.WriteLine(this);
        }

        public Board(uint hei, uint wid) {
            this.height = hei;
            this.width = wid;
            this.board_state = new int[this.height, this.width];
            Console.WriteLine("Board created with height = {0}, width = {1}.", this.height, this.width);
            Console.WriteLine(this);
        }

        public void add_ship(uint top, uint left, uint len, char dir) {
            Console.WriteLine("Adding ship for position ({0}, {1}) with length of {2} and direction {3}.", top, left, len, dir);
            if (len == 0) {
                return;
            }
            if (top >= this.height || left >= this.width) {
                Console.WriteLine("Position not valid.");
                return;
            }
            switch (dir) {
                case 'v':
                    if (top + len > this.height) {
                        Console.WriteLine("This ship is too long for height.");
                        return;
                    }
                    for (uint i = top; i < top + len; i++) {
                        if (this.board_state[i, left] != 0) {
                            Console.WriteLine("This ship is overlapped with en existing ship.");
                            return;
                        }
                    }
                    for (uint i = top; i < top + len; i++) {
                        this.board_state[i, left] = 1;
                    }
                    this.num_of_ship_blocks += len;
                    Console.WriteLine(this);
                    break;
                case 'h':
                    if (left + len > this.width) {
                        Console.WriteLine("This ship is too long for width.");
                        return;
                    }
                    for (uint i = left; i < left + len; i++) {
                        if (this.board_state[top, i] != 0) {
                            Console.WriteLine("This ship is overlapped with en existing ship.");
                            return;
                        }
                    }
                    for (uint i = left; i < left + len; i++) {
                        this.board_state[top, i] = 1;
                    }
                    this.num_of_ship_blocks += len;
                    Console.WriteLine(this);
                    break;
                default:
                    Console.WriteLine("Direction should be 'v' or 'h'.");
                    break;
            }
        }

        public bool attack(uint hei, uint wid) {
            if (hei >= this.height || wid >= this.width) {
                Console.WriteLine("Position not valid.");
                return true;
            }
            if (this.board_state[hei, wid] == 1) {
                this.board_state[hei, wid] = 2;
                this.num_of_ship_blocks -= 1;
                Console.WriteLine("Hit!");
                Console.WriteLine(this);
                if (this.num_of_ship_blocks == 0) {
                    Console.WriteLine("There is no ship in the board.");
                    Console.WriteLine("You lose!");
                    return false;
                }
                return true;
            } else {
                Console.WriteLine("Miss!");
                return true;
            }
        }

        public override String ToString() {
            String ret = "";
            for (uint i = 0; i < this.board_state.GetLength(0); i++) {
                for (uint j = 0; j < this.board_state.GetLength(1); j++) {
                    ret += this.board_state[i, j];
                }
                if (i != this.board_state.GetLength(0) - 1) {
                    ret += Environment.NewLine;
                }
            }
            return ret;
        }

    }

    class Battleship
    {
        static void Main(string[] args)
        {
            uint height = 0;
            uint width = 0;
            String[] input_array;
            do {
                Console.WriteLine("Please input the board size, use space to seperate:");
                String input = Console.ReadLine();
                input_array = input.Split(new Char[] { ' ', ',', '.', '-', '\n', '\t'}, StringSplitOptions.RemoveEmptyEntries);
            } while (!(input_array.GetLength(0) == 2 && uint.TryParse(input_array[0], out height) && uint.TryParse(input_array[1], out width)));
            
            Board board = new Board(height, width);

            bool flag = true;
            do {

                Console.WriteLine("Please input the operation followed by the arguments, use space to seperate:");
                String input = Console.ReadLine();
                input_array = input.Split(new Char[] { ' ', ',', '.', '-', '\n', '\t'}, StringSplitOptions.RemoveEmptyEntries);
                if (input_array[0] == "addship") {
                    uint top = 0;
                    uint left = 0;
                    uint len = 0;
                    char dir = '0';
                    if (input_array.GetLength(0) == 5 && uint.TryParse(input_array[1], out top) && uint.TryParse(input_array[2], out left)
                            && uint.TryParse(input_array[3], out len) && char.TryParse(input_array[4], out dir)) {
                        board.add_ship(top, left, len, dir);
                    } else {
                        Console.WriteLine("Addship arguments incorrect. Should be top, left, length, direction.");
                    }
                }
                else if (input_array[0] == "attack") {
                    uint hei = 0;
                    uint wid = 0;
                    if (input_array.GetLength(0) == 3 && uint.TryParse(input_array[1], out hei) && uint.TryParse(input_array[2], out wid)) {
                        flag = board.attack(hei, wid);
                    } else {
                        Console.WriteLine("Attack arguments incorrect. Should be height, width.");
                    }
                }
                else {
                    Console.WriteLine("No such operation.");
                }
            } while (flag);
            
        }
    }
}
