using System;

namespace CSharpColorSpaceConverter
{
    public static class ColorSpaceConverter
    {
        public static double[] RGBToHSL(int r, int g, int b)
        {
            double R = r / 255d;
            double G = g / 255d;
            double B = b / 255d;

            double min = Math.Min(R, Math.Min(G, B));
            double max = Math.Max(R, Math.Max(G, B));
            double deltaMax = max - min;

            double H = 0;
            double S = 0;
            double L = (min + max) / 2d;

            if (deltaMax != 0)
            {
                if (L < .5)
                {
                    S = deltaMax / (max + min);
                }
                else
                {
                    S = deltaMax / (2 - max - min);
                }
            }

            double deltaR = (((max - R) / 6d) + (max / 2d)) / deltaMax;
            double deltaG = (((max - G) / 6d) + (max / 2d)) / deltaMax;
            double deltaB = (((max - B) / 6d) + (max / 2d)) / deltaMax;

            if (R == max)
            {
                H = deltaB - deltaG;
            }
            else if (G == max)
            {
                H = (1 / 3d) + deltaR - deltaB;
            }
            else if (B == max)
            {
                H = (2 / 3d) + deltaG - deltaR;
            }

            return new double[] { H, S, L };
        }

        public static int[] HSLToRGB(double h, double s, double l)
        {
            double R = 0;
            double G = 0;
            double B = 0;

            if (s == 0)
            {
                R = l * 255;
                G = l * 255;
                B = l * 255;
            }
            else
            {
                double var2 = 0;
                if (l < .5)
                {
                    var2 = l * (1 + s);
                }
                else
                {
                    var2 = (l + s) - (s * l);
                }
                double var1 = 2 * l - var2;

                R = 255 * HueToRGB(var1, var2, h + (1 / 3d));
                G = 255 * HueToRGB(var1, var2, h);
                B = 255 * HueToRGB(var1, var2, h - (1 / 3d));
            }

            int r = Convert.ToInt32(R);
            int g = Convert.ToInt32(G);
            int b = Convert.ToInt32(B);
            return new int[] { r, g, b };
        }

        static double HueToRGB(double v1, double v2, double vH)
        {
            if (vH < 0)
            {
                vH += 1;
            }
            if (vH > 1)
            {
                vH -= 1;
            }
            if ((6 * vH) < 1)
            {
                return (v1 + (v2 - v1) * 6 * vH);
            }
            if ((2 * vH) < 1)
            {
                return v2;
            }
            if ((3 * vH) < 2)
            {
                return (v1 + (v2 - v1) * ((2 / 3d) - vH) * 6);
            }
            return v1;
        }

        public static double[] RGBToLab(int r, int g, int b)
        {
            double[] xyz = RGBToXYZ(r, g, b);
            double[] lab = XYZToLab(xyz[0], xyz[1], xyz[2]);
            return lab;
        }

        public static double[] RGBToXYZ(double r, double g, double b)
        {
            //sR, sG and sB (Standard RGB) input range = 0 ÷ 255
            //X, Y and Z output refer to a D65/2° standard illuminant.

            r /= 255f;
            g /= 255f;
            b /= 255f;

            if (r > 0.04045)
            {
                r = Math.Pow(((r + 0.055) / 1.055), 2.4);
            }
            else
            {
                r /= 12.92;
            }
            if (g > 0.04045)
            {
                g = Math.Pow(((g + 0.055) / 1.055), 2.4);
            }
            else
            {
                g /= 12.92;
            }
            if (b > 0.04045)
            {
                b = Math.Pow(((b + 0.055) / 1.055), 2.4);
            }
            else
            {
                b /= 12.92;
            }

            r *= 100f;
            g *= 100f;
            b *= 100f;

            double x = r * 0.4124 + g * 0.3576 + b * 0.1805;
            double y = r * 0.2126 + g * 0.7152 + b * 0.0722;
            double z = r * 0.0193 + g * 0.1192 + b * 0.9505;

            return new double[] { x, y, z };
        }

        public static double[] XYZToLab(double x, double y, double z)
        {
            //Reference-X, Y and Z refer to specific illuminants and observers.
            //Common reference values are available below in this same page. http://www.easyrgb.com/en/math.php

            x /= 111.144;
            y /= 100.000;
            z /= 35.200;

            if (x > 0.008856)
            {
                x = Math.Pow(x, (1f / 3f));
            }
            else
            {
                x = (7.787 * x) + (16f / 116f);
            }
            if (y > 0.008856)
            {
                y = Math.Pow(y, (1f / 3f));
            }
            else
            {
                y = (7.787 * y) + (16f / 11f);
            }
            if (z > 0.008856)
            {
                z = Math.Pow(z, (1f / 3f));
            }
            else
            {
                z = (7.787 * z) + (16f / 116f);
            }

            double l = (116 * y) - 16;
            double a = 500 * (x - y);
            double b = 200 * (y - z);

            return new double[] { l, a, b };
        }

        static char[] hexDigits = {
         '0', '1', '2', '3', '4', '5', '6', '7',
         '8', '9', 'A', 'B', 'C', 'D', 'E', 'F'};

        public static string RGBToHex(int r, int g, int b)
        {
            byte[] bytes = new byte[3];
            bytes[0] = (byte)r;
            bytes[1] = (byte)g;
            bytes[2] = (byte)b;
            char[] chars = new char[bytes.Length * 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                int byt = bytes[i];
                chars[i * 2] = hexDigits[byt >> 4];
                chars[i * 2 + 1] = hexDigits[byt & 0xF];
            }
            return new string(chars);
        }

        static string[] names = new string[] { "Aero", "Aero blue", "African violet", "Air Force blue (RAF)", "Air Force blue (USAF)", "Air superiority blue", "Alabama Crimson", "Alice blue", "Alizarin crimson", "Alloy orange", "Almond", "Amaranth", "Amazon", "Amber", "SAE/ECE Amber (color)", "American rose", "Amethyst", "Android green", "Anti-flash white", "Antique brass", "Antique bronze", "Antique fuchsia", "Antique ruby", "Antique white", "Ao (English)", "Apple green", "Apricot", "Aqua", "Aquamarine", "Army green", "null", "Arylide yellow", "Ash grey", "Asparagus", "Atomic tangerine", "Auburn", "Aureolin", "AuroMetalSaurus", "Avocado", "Azure", "Azure mist/web", "Baby blue", "Baby blue eyes", "Baby pink", "Baby powder", "Baker-Miller pink", "Ball blue", "Banana Mania", "Banana yellow", "Barbie pink", "Barn red", "Battleship grey", "Bazaar", "Beau blue", "Beaver", "Beige", "B'dazzled Blue", "Big dip o�ruby", "Bisque", "Bistre", "Bistre brown", "Bitter lemon", "Bitter lime", "Bittersweet", "Bittersweet shimmer", "Black", "Black bean", "Black leather jacket", "Black olive", "Blanched almond", "Blast-off bronze", "Bleu de France", "Blizzard Blue", "Blond", "Blue", "Blue (Crayola)", "Blue (Munsell)", "Blue (NCS)", "Blue (pigment)", "Blue (RYB)", "Blue Bell", "Blue-gray", "Blue-green", "Blue sapphire", "Blue-violet", "Blueberry", "Bluebonnet", "Blush", "Bole", "Bondi blue", "Bone", "Boston University Red", "Bottle green", "Boysenberry", "Brandeis blue", "Brass", "Brick red", "Bright cerulean", "Bright green", "Bright lavender", "Bright maroon", "Bright pink", "Bright turquoise", "Bright ube", "Brilliant lavender", "Brilliant rose", "Brink pink", "British racing green", "Bronze", "Bronze Yellow", "Brown (traditional)", "Brown (web)", "Brown-nose", "Brunswick green", "Bubble gum", "Bubbles", "Buff", "Bulgarian rose", "Burgundy", "Burlywood", "Burnt orange", "Burnt sienna", "Burnt umber", "Byzantine", "Byzantium", "Cadet", "Cadet blue", "Cadet grey", "Cadmium green", "Cadmium orange", "Cadmium red", "Cadmium yellow", "Caf� au lait", "Caf� noir", "Cal Poly green", "Cambridge Blue", "Camel", "Cameo pink", "Camouflage green", "Canary yellow", "Candy apple red", "Candy pink", "Capri", "Caput mortuum", "Cardinal", "Caribbean green", "Carmine", "Carmine (M&amp;P)", "Carmine pink", "Carmine red", "Carnation pink", "Carnelian", "Carolina blue", "Carrot orange", "Castleton green", "Catalina blue", "Cedar Chest", "Ceil", "Celadon", "Celadon blue", "Celadon green", "Celeste (colour)", "Celestial blue", "Cerise", "Cerise pink", "Cerulean", "Cerulean blue", "Cerulean frost", "CG Blue", "CG Red", "Chamoisee", "Champagne", "Charcoal", "Charleston green", "Charm pink", "Chartreuse (traditional)", "Chartreuse (web)", "Cherry", "Cherry blossom pink", "Chestnut", "China pink", "China rose", "Chinese red", "Chinese violet", "Chocolate (traditional)", "Chocolate (web)", "Chrome yellow", "Cinereous", "Cinnabar", "Cinnamon", "Citrine", "Citron", "Claret", "Classic rose", "Cobalt", "Cocoa brown", "Coconut", "Coffee", "Columbia blue", "Congo pink", "Cool black", "Cool grey", "Copper", "Copper (Crayola)", "Copper penny", "Copper red", "Copper rose", "Coquelicot", "Coral", "Coral pink", "Coral red", "Cordovan", "Corn", "Cornell Red", "Cornflower blue", "Cornsilk", "Cosmic latte", "Cotton candy", "Cream", "Crimson", "Crimson glory", "Cyan", "Cyan (process)", "Cyber grape", "Daffodil", "Dandelion", "Dark blue", "Dark blue-gray", "Dark brown", "Dark byzantium", "Dark candy apple red", "Dark cerulean", "Dark chestnut", "Dark coral", "Dark cyan", "Dark electric blue", "Dark goldenrod", "Dark gray", "Dark green", "Dark imperial blue", "Dark jungle green", "Dark khaki", "Dark lava", "Dark lavender", "Dark magenta", "Dark midnight blue", "Dark moss green", "Dark olive green", "Dark orange", "Dark orchid", "Dark pastel blue", "Dark pastel green", "Dark pastel purple", "Dark pastel red", "Dark pink", "Dark powder blue", "Dark raspberry", "Dark red", "Dark salmon", "Dark scarlet", "Dark sea green", "Dark sienna", "Dark sky blue", "Dark slate blue", "Dark slate gray", "Dark spring green", "Dark tan", "Dark tangerine", "Dark taupe", "Dark terra cotta", "Dark turquoise", "Dark vanilla", "Dark violet", "Dark yellow", "Dartmouth green", "Davy's grey", "Debian red", "Deep carmine", "Deep carmine pink", "Deep carrot orange", "Deep cerise", "Deep champagne", "Deep chestnut", "Deep coffee", "Deep fuchsia", "Deep jungle green", "Deep lemon", "Deep lilac", "Deep magenta", "Deep mauve", "Deep moss green", "Deep peach", "Deep pink", "Deep ruby", "Deep saffron", "Deep sky blue", "Deep Space Sparkle", "Deep Taupe", "Deep Tuscan red", "Deer", "Denim", "Desert", "Desert sand", "Diamond", "Dim gray", "Dirt", "Dodger blue", "Dogwood rose", "Dollar bill", "Drab", "Duke blue", "Dust storm", "Earth yellow", "Ebony", "Ecru", "Eggplant", "Eggshell", "Egyptian blue", "Electric blue", "Electric crimson", "Electric cyan", "Electric green", "Electric indigo", "Electric lavender", "Electric lime", "Electric purple", "Electric ultramarine", "Electric violet", "Electric yellow", "Emerald", "English green", "English lavender", "English red", "English violet", "Eton blue", "Eucalyptus", "Fallow", "Falu red", "Fandango", "Fandango pink", "Fashion fuchsia", "Fawn", "Feldgrau", "Feldspar", "Fern green", "Ferrari Red", "Field drab", "Firebrick", "Fire engine red", "Flame", "Flamingo pink", "Flattery", "Flavescent", "Flax", "Floral white", "Fluorescent orange", "Fluorescent pink", "Fluorescent yellow", "Folly", "Forest green (traditional)", "Forest green (web)", "French beige", "French bistre", "French blue", "French lilac", "French lime", "French mauve", "French raspberry", "French rose", "French sky blue", "French wine", "Fresh Air", "Fuchsia", "Fuchsia (Crayola)", "Fuchsia pink", "Fuchsia rose", "Fulvous", "Fuzzy Wuzzy", "Gainsboro", "Gamboge", "Ghost white", "Giants orange", "Ginger", "Glaucous", "Glitter", "GO green", "Gold (metallic)", "Gold (web) (Golden)", "Gold Fusion", "Golden brown", "Golden poppy", "Golden yellow", "Goldenrod", "Granny Smith Apple", "Grape", "Gray", "Gray (HTML/CSS gray)", "Gray (X11 gray)", "Gray-asparagus", "Gray-blue", "Green (color wheel) (X11 green)", "Green (Crayola)", "Green (HTML/CSS color)", "Green (Munsell)", "Green (NCS)", "Green (pigment)", "Green (RYB)", "Green-yellow", "Grullo", "Guppie green", "Halay� �be", "Han blue", "Han purple", "Hansa yellow", "Harlequin", "Harvard crimson", "Harvest gold", "Heart Gold", "Heliotrope", "Hollywood cerise", "Honeydew", "Honolulu blue", "Hooker's green", "Hot magenta", "Hot pink", "Hunter green", "Iceberg", "Icterine", "Illuminating Emerald", "Imperial", "Imperial blue", "Imperial purple", "Imperial red", "Inchworm", "India green", "Indian red", "Indian yellow", "Indigo", "Indigo (dye)", "Indigo (web)", "International Klein Blue", "International orange (aerospace)", "International orange (engineering)", "International orange (Golden Gate Bridge)", "Iris", "Irresistible", "Isabelline", "Islamic green", "Italian sky blue", "Ivory", "Jade", "Japanese indigo", "Japanese violet", "Jasmine", "Jasper", "Jazzberry jam", "Jelly Bean", "Jet", "Jonquil", "June bud", "Jungle green", "Kelly green", "Kenyan copper", "Keppel", "Khaki (HTML/CSS) (Khaki)", "Khaki (X11) (Light khaki)", "Kobe", "Kobi", "KU Crimson", "La Salle Green", "Languid lavender", "Lapis lazuli", "Laser Lemon", "Laurel green", "Lava", "Lavender (floral)", "Lavender (web)", "Lavender blue", "Lavender blush", "Lavender gray", "Lavender indigo", "Lavender magenta", "Lavender mist", "Lavender pink", "Lavender purple", "Lavender rose", "Lawn green", "Lemon", "Lemon chiffon", "Lemon curry", "Lemon glacier", "Lemon lime", "Lemon meringue", "Lemon yellow", "Licorice", "Light apricot", "Light blue", "Light brown", "Light carmine pink", "Light coral", "Light cornflower blue", "Light crimson", "Light cyan", "Light fuchsia pink", "Light goldenrod yellow", "Light gray", "Light green", "Light khaki", "Light medium orchid", "Light moss green", "Light orchid", "Light pastel purple", "Light pink", "Light red ochre", "Light salmon", "Light salmon pink", "Light sea green", "Light sky blue", "Light slate gray", "Light steel blue", "Light taupe", "Light Thulian pink", "Light yellow", "Lilac", "Lime (color wheel)", "Lime (web) (X11 green)", "Lime green", "Limerick", "Lincoln green", "Linen", "Lion", "Little boy blue", "Liver", "Lumber", "Lust", "Magenta", "Magenta (Crayola)", "Magenta (dye)", "Magenta (Pantone)", "Magenta (process)", "Magic mint", "Magnolia", "Mahogany", "Maize", "Majorelle Blue", "Malachite", "Manatee", "Mango Tango", "Mantis", "Mardi Gras", "Maroon (Crayola)", "Maroon (HTML/CSS)", "Maroon (X11)", "Mauve", "Mauve taupe", "Mauvelous", "Maya blue", "Meat brown", "Medium aquamarine", "Medium blue", "Medium candy apple red", "Medium carmine", "Medium champagne", "Medium electric blue", "Medium jungle green", "Medium lavender magenta", "Medium orchid", "Medium Persian blue", "Medium purple", "Medium red-violet", "Medium ruby", "Medium sea green", "Medium sky blue", "Medium slate blue", "Medium spring bud", "Medium spring green", "Medium taupe", "Medium turquoise", "Medium Tuscan red", "Medium vermilion", "Medium violet-red", "Mellow apricot", "Mellow yellow", "Melon", "Metallic Seaweed", "Metallic Sunburst", "Mexican pink", "Midnight blue", "Midnight green (eagle green)", "Midori", "Mikado yellow", "Mint", "Mint cream", "Mint green", "Misty rose", "Moccasin", "Mode beige", "Moonstone blue", "Mordant red 19", "Moss green", "Mountain Meadow", "Mountbatten pink", "MSU Green", "Mughal green", "Mulberry", "Mustard", "Myrtle green", "Nadeshiko pink", "Napier green", "Naples yellow", "Navajo white", "Navy blue", "Navy purple", "Neon Carrot", "Neon fuchsia", "Neon green", "New Car", "New York pink", "Non-photo blue", "North Texas Green", "Nyanza", "Ocean Boat Blue", "Ochre", "Office green", "Old burgundy", "Old gold", "Old lace", "Old lavender", "Old mauve", "Old moss green", "Old rose", "Old silver", "Olive", "Olive Drab (web) (Olive Drab #3)", "Olive Drab #7", "Olivine", "Onyx", "Opera mauve", "Orange (color wheel)", "Orange (RYB)", "Orange (web color)", "Orange peel", "Orange-red", "Orchid", "Orchid pink", "Orioles orange", "Otter brown", "Outer Space", "Outrageous Orange", "Oxford Blue", "OU Crimson Red", "Pakistan green", "Palatinate blue", "Palatinate purple", "Pale aqua", "Pale blue", "Pale brown", "Pale carmine", "Pale cerulean", "Pale chestnut", "Pale copper", "Pale cornflower blue", "Pale gold", "Pale goldenrod", "Pale green", "Pale lavender", "Pale magenta", "Pale pink", "Pale plum", "Pale red-violet", "Pale robin egg blue", "Pale silver", "Pale spring bud", "Pale taupe", "Pale turquoise", "Pale violet-red", "Pansy purple", "Papaya whip", "Paris Green", "Pastel blue", "Pastel brown", "Pastel gray", "Pastel green", "Pastel magenta", "Pastel orange", "Pastel pink", "Pastel purple", "Pastel red", "Pastel violet", "Pastel yellow", "Patriarch", "Payne's grey", "Peach", "Peach (Crayola)", "Peach-orange", "Peach puff", "Peach-yellow", "Pear", "Pearl", "Pearl Aqua", "Pearly purple", "Peridot", "Periwinkle", "Persian blue", "Persian green", "Persian indigo", "Persian orange", "Persian pink", "Persian plum", "Persian red", "Persian rose", "Persimmon", "Peru", "Phlox", "Phthalo blue", "Phthalo green", "Pictorial carmine", "Piggy pink", "Pine green", "Pink", "Pink lace", "null", "Pink pearl", "Pink Sherbet", "Pistachio", "Platinum", "Plum (traditional)", "Plum (web)", "Pomp and Power", "Portland Orange", "Powder blue (web)", "Princeton orange", "Prune", "Prussian blue", "Psychedelic purple", "Puce", "Pumpkin", "Purple (HTML/CSS)", "Purple (Munsell)", "Purple (X11)", "Purple Heart", "Purple mountain majesty", "Purple pizzazz", "Purple taupe", "Quartz", "Queen blue", "Queen pink", "Rackley", "Radical Red", "Rajah", "Raspberry", "Raspberry glace", "Raspberry pink", "Raspberry rose", "Raw umber", "Razzle dazzle rose", "Razzmatazz", "Razzmic Berry", "Red", "Red (Munsell)", "Red (NCS)", "Red (pigment)", "Red (RYB)", "Red-brown", "Red devil", "Red-orange", "Red-violet", "Redwood", "Regalia", "Resolution blue", "Rhythm", "Rich black", "Rich brilliant lavender", "Rich carmine", "Rich electric blue", "Rich lavender", "Rich lilac", "Rich maroon", "Rifle green", "Robin egg blue", "Rocket metallic", "Roman silver", "Rose", "Rose bonbon", "Rose ebony", "Rose gold", "Rose madder", "Rose pink", "Rose quartz", "Rose taupe", "Rose vale", "Rosewood", "Rosso corsa", "Rosy brown", "Royal azure", "Royal blue (traditional)", "Royal blue (web)", "Royal fuchsia", "Royal purple", "Royal yellow", "Ruber", "Rubine red", "Ruby", "Ruby red", "Ruddy", "Ruddy brown", "Ruddy pink", "Rufous", "Russet", "Russian violet", "Rust", "Rusty red", "Sacramento State green", "Saddle brown", "Safety orange (blaze orange)", "Saffron", "St. Patrick's blue", "Salmon", "Salmon pink", "Sand", "Sand dune", "Sandstorm", "Sandy brown", "Sandy taupe", "Sangria", "Sap green", "Sapphire", "Sapphire blue", "Satin sheen gold", "Scarlet", "Scarlet (Crayola)", "Schauss pink", "School bus yellow", "Screamin' Green", "Sea blue", "Sea green", "Seal brown", "Seashell", "Selective yellow", "Sepia", "Shadow", "Shampoo", "Shamrock green", "Sheen Green", "Shimmering Blush", "Shocking pink", "Shocking pink (Crayola)", "Sienna", "Silver", "Silver chalice", "Silver pink", "Silver sand", "Sinopia", "Skobeloff", "Sky blue", "Sky magenta", "Slate blue", "Slate gray", "Smalt (Dark powder blue)", "Smitten", "Smoke", "Smokey topaz", "Smoky black", "Snow", "Soap", "Sonic silver", "Spartan Crimson", "Space cadet", "Spanish bistre", "Spanish carmine", "Spanish crimson", "Spanish orange", "Spanish sky blue", "Spiro Disco Ball", "Spring bud", "Spring green", "Star command blue", "Steel blue", "Steel pink", "Stil de grain yellow", "Stizza", "Stormcloud", "Straw", "Strawberry", "Sunglow", "Sunset", "Super pink", "Tan", "Tangelo", "Tangerine", "Tangerine yellow", "Tango pink", "Taupe", "Taupe gray", "Tea green", "Tea rose (orange)", "Tea rose (rose)", "Teal", "Teal blue", "Teal deer", "Teal green", "Telemagenta", "Tenn� (Tawny)", "Terra cotta", "Thistle", "Thulian pink", "Tickle Me Pink", "Tiffany Blue", "Tiger's eye", "Timberwolf", "Titanium yellow", "Tomato", "Toolbox", "Topaz", "Tractor red", "Trolley Grey", "Tropical rain forest", "True Blue", "Tufts Blue", "Tulip", "Tumbleweed", "Turkish rose", "Turquoise", "Turquoise blue", "Turquoise green", "Tuscan", "Tuscan brown", "Tuscan red", "Tuscan tan", "Tuscany", "Twilight lavender", "Tyrian purple", "UA blue", "UA red", "Ube", "UCLA Blue", "UCLA Gold", "UFO Green", "Ultramarine", "Ultramarine blue", "Ultra pink", "Umber", "Unbleached silk", "United Nations blue", "University of California Gold", "Unmellow Yellow", "UP Forest green", "UP Maroon", "Upsdell red", "Urobilin", "USAFA blue", "USC Cardinal", "USC Gold", "University of Tennessee Orange", "Utah Crimson", "Vanilla", "Vanilla ice", "Vegas gold", "Venetian red", "Verdigris", "Vermilion (cinnabar)", "Vermilion (Plochere)", "Veronica", "Violet", "Violet (color wheel)", "Violet (RYB)", "Violet (web)", "Violet-blue", "Violet-red", "Viridian", "Vivid auburn", "Vivid burgundy", "Vivid cerise", "Vivid orchid", "Vivid sky blue", "Vivid tangerine", "Vivid violet", "Warm black", "Waterspout", "Wenge", "Wheat", "White", "White smoke", "Wild blue yonder", "Wild orchid", "Wild Strawberry", "Wild Watermelon", "Windsor tan", "Wine", "Wine dregs", "Wisteria", "Wood brown", "Xanadu", "Yale Blue", "Yankees blue", "Yellow", "Yellow (Munsell)", "Yellow (NCS)", "Yellow (process)", "Yellow (RYB)", "Yellow-green", "Yellow Orange", "Yellow rose", "Zaffre", "Zinnwaldite brown", "Zomp" };
        static int[] namesR = new int[] { 124, 201, 178, 91, 0, 114, 163, 239, 226, 196, 239, 229, 58, 255, 255, 255, 153, 163, 242, 204, 102, 145, 132, 249, 0, 140, 249, 0, 127, 73, 58, 232, 178, 135, 255, 165, 252, 109, 86, 0, 239, 137, 160, 244, 255, 255, 33, 249, 255, 224, 124, 132, 153, 188, 158, 244, 45, 155, 255, 61, 150, 201, 99, 255, 191, 0, 61, 38, 58, 255, 165, 48, 170, 249, 0, 30, 0, 0, 51, 2, 163, 102, 12, 17, 137, 79, 28, 221, 119, 0, 226, 204, 0, 135, 0, 181, 204, 28, 102, 191, 193, 255, 7, 209, 244, 255, 249, 0, 204, 114, 150, 165, 102, 28, 255, 232, 239, 71, 127, 221, 204, 232, 137, 188, 112, 84, 94, 145, 0, 237, 226, 255, 165, 73, 30, 163, 193, 239, 119, 255, 255, 226, 0, 89, 196, 0, 150, 214, 234, 255, 255, 178, 153, 237, 0, 5, 201, 145, 170, 0, 45, 178, 73, 221, 237, 0, 40, 109, 0, 224, 160, 247, 53, 35, 229, 221, 127, 221, 255, 142, 221, 168, 170, 132, 122, 209, 255, 153, 226, 209, 226, 158, 127, 249, 0, 209, 150, 112, 155, 247, 0, 140, 183, 216, 173, 204, 153, 255, 255, 247, 255, 137, 249, 178, 99, 255, 255, 255, 255, 219, 191, 0, 0, 89, 255, 239, 0, 102, 102, 91, 163, 7, 153, 204, 0, 84, 183, 168, 0, 0, 25, 188, 71, 114, 140, 0, 73, 84, 255, 153, 119, 2, 150, 193, 232, 0, 135, 140, 232, 86, 142, 61, 140, 71, 45, 22, 145, 255, 71, 204, 0, 209, 147, 155, 0, 84, 214, 168, 239, 232, 216, 249, 186, 112, 193, 0, 244, 153, 204, 211, 53, 255, 255, 132, 255, 0, 73, 124, 102, 186, 20, 193, 237, 186, 104, 155, 30, 214, 132, 150, 0, 229, 224, 84, 193, 96, 239, 15, 124, 255, 0, 0, 112, 244, 204, 191, 63, 142, 255, 79, 28, 181, 170, 86, 150, 68, 193, 127, 181, 221, 244, 229, 76, 224, 79, 255, 107, 178, 206, 226, 252, 102, 247, 237, 255, 255, 255, 204, 255, 0, 33, 165, 132, 0, 135, 158, 211, 198, 244, 119, 170, 165, 255, 193, 255, 198, 226, 204, 219, 226, 247, 255, 175, 96, 229, 0, 211, 255, 132, 153, 252, 255, 216, 168, 107, 127, 127, 191, 68, 140, 0, 28, 0, 0, 0, 0, 102, 173, 168, 0, 102, 68, 81, 232, 63, 201, 216, 127, 221, 244, 239, 0, 73, 255, 255, 53, 112, 252, 48, 96, 0, 102, 237, 178, 17, 204, 226, 112, 0, 73, 0, 255, 186, 191, 89, 178, 244, 0, 178, 255, 0, 38, 91, 247, 214, 165, 216, 51, 244, 188, 40, 76, 124, 58, 193, 239, 135, 232, 232, 7, 214, 38, 255, 168, 206, 181, 229, 204, 255, 196, 147, 237, 229, 249, 150, 249, 124, 255, 255, 204, 252, 226, 244, 255, 25, 252, 173, 181, 229, 239, 147, 244, 224, 249, 249, 211, 142, 239, 211, 173, 229, 175, 255, 232, 255, 255, 33, 135, 119, 175, 178, 229, 255, 198, 191, 0, 51, 158, 25, 249, 193, 107, 84, 255, 229, 255, 255, 201, 209, 255, 170, 247, 191, 249, 96, 10, 150, 255, 114, 135, 193, 127, 175, 224, 145, 239, 114, 229, 102, 0, 226, 175, 242, 2, 28, 221, 186, 0, 147, 186, 170, 61, 127, 122, 201, 0, 102, 71, 119, 216, 198, 247, 247, 252, 7, 155, 226, 25, 0, 226, 255, 61, 244, 153, 255, 249, 150, 114, 173, 137, 48, 153, 22, 48, 196, 255, 48, 244, 40, 249, 255, 0, 147, 255, 255, 56, 33, 214, 163, 5, 232, 0, 204, 0, 66, 206, 252, 119, 102, 135, 191, 132, 127, 107, 61, 153, 53, 183, 255, 249, 255, 255, 255, 216, 242, 249, 102, 63, 255, 0, 153, 0, 38, 104, 188, 175, 153, 175, 155, 221, 216, 170, 229, 237, 153, 219, 249, 249, 221, 219, 150, 201, 237, 188, 175, 219, 119, 255, 79, 173, 130, 206, 119, 244, 255, 221, 178, 255, 204, 252, 127, 84, 255, 255, 255, 255, 249, 209, 234, 135, 183, 229, 204, 28, 0, 51, 216, 247, 112, 204, 255, 237, 204, 221, 0, 17, 193, 252, 0, 255, 255, 255, 232, 247, 147, 229, 142, 204, 135, 255, 175, 255, 112, 0, 221, 204, 255, 127, 158, 160, 104, 150, 255, 79, 81, 66, 232, 91, 255, 249, 226, 145, 226, 178, 130, 255, 226, 140, 255, 242, 196, 237, 255, 165, 135, 255, 198, 170, 81, 0, 119, 0, 242, 214, 7, 165, 181, 175, 63, 0, 137, 130, 255, 249, 102, 183, 226, 255, 170, 142, 170, 102, 211, 188, 0, 0, 63, 201, 119, 249, 206, 209, 224, 155, 255, 186, 224, 168, 127, 51, 183, 216, 0, 140, 255, 244, 35, 255, 255, 193, 150, 237, 244, 150, 145, 79, 15, 0, 204, 255, 252, 255, 255, 117, 0, 45, 51, 255, 255, 112, 137, 255, 0, 142, 216, 252, 255, 135, 191, 170, 191, 191, 204, 0, 135, 206, 107, 112, 0, 198, 114, 147, 15, 255, 206, 117, 158, 28, 127, 209, 229, 232, 0, 15, 165, 0, 0, 68, 204, 249, 153, 79, 226, 252, 255, 249, 206, 209, 249, 242, 255, 226, 71, 140, 209, 247, 244, 0, 53, 153, 0, 206, 204, 226, 216, 221, 252, 10, 224, 219, 237, 255, 114, 255, 252, 127, 0, 0, 63, 255, 221, 181, 48, 0, 160, 249, 112, 124, 160, 191, 137, 102, 0, 216, 135, 84, 255, 61, 17, 63, 255, 99, 255, 91, 183, 255, 0, 122, 173, 224, 0, 153, 255, 247, 211, 242, 242, 196, 198, 66, 226, 216, 160, 142, 127, 135, 237, 51, 247, 63, 145, 158, 216, 204, 0, 255, 158, 0, 163, 99, 244, 255, 244, 163, 214, 255, 252, 165, 114, 102, 201, 193, 114, 15, 28, 255, 239, 255, 255, 255, 153, 255, 255, 0, 43, 56 };
        static int[] namesG = new int[] { 186, 255, 132, 137, 48, 160, 38, 247, 38, 96, 221, 43, 122, 191, 124, 2, 102, 198, 242, 147, 91, 91, 28, 234, 127, 181, 206, 255, 255, 84, 68, 214, 191, 168, 153, 40, 237, 127, 130, 127, 255, 206, 201, 193, 255, 145, 170, 232, 224, 33, 10, 132, 119, 211, 130, 244, 89, 38, 226, 43, 112, 224, 140, 112, 79, 0, 12, 53, 61, 234, 112, 140, 229, 239, 0, 117, 147, 135, 51, 71, 163, 153, 153, 96, 43, 135, 28, 91, 68, 147, 216, 0, 107, 51, 112, 165, 63, 170, 255, 147, 33, 0, 232, 158, 186, 84, 96, 66, 127, 112, 73, 40, 68, 76, 193, 255, 219, 5, 0, 183, 84, 114, 51, 51, 40, 104, 158, 163, 107, 135, 0, 244, 122, 53, 76, 193, 153, 186, 135, 239, 7, 112, 191, 38, 30, 204, 0, 0, 76, 0, 165, 28, 186, 145, 86, 40, 89, 160, 224, 122, 132, 255, 150, 48, 58, 122, 81, 155, 122, 61, 119, 232, 68, 43, 142, 255, 255, 48, 183, 68, 112, 81, 56, 96, 63, 104, 165, 130, 66, 104, 209, 168, 22, 204, 71, 104, 89, 79, 221, 130, 45, 145, 114, 137, 112, 109, 102, 56, 127, 130, 63, 63, 237, 28, 147, 247, 247, 188, 252, 20, 0, 255, 183, 66, 255, 224, 0, 102, 66, 56, 0, 68, 104, 91, 140, 104, 135, 168, 51, 63, 35, 183, 61, 79, 0, 51, 91, 107, 140, 51, 158, 191, 112, 58, 84, 51, 38, 0, 150, 2, 188, 20, 191, 61, 79, 114, 130, 168, 61, 79, 206, 191, 0, 135, 112, 84, 10, 33, 48, 104, 51, 214, 79, 66, 84, 73, 198, 84, 0, 114, 94, 204, 20, 63, 153, 191, 99, 94, 66, 135, 96, 153, 201, 242, 104, 117, 142, 22, 186, 112, 0, 204, 168, 91, 178, 63, 234, 51, 249, 0, 255, 255, 0, 186, 255, 0, 0, 0, 255, 198, 76, 130, 79, 61, 198, 214, 153, 22, 51, 81, 0, 170, 91, 181, 119, 40, 84, 33, 33, 89, 142, 68, 232, 219, 249, 191, 20, 255, 0, 68, 140, 122, 109, 114, 96, 252, 114, 43, 73, 181, 30, 232, 0, 84, 119, 66, 132, 102, 219, 155, 247, 89, 102, 130, 232, 170, 175, 214, 117, 102, 193, 221, 165, 226, 45, 127, 127, 191, 89, 145, 255, 170, 127, 168, 158, 165, 175, 255, 153, 255, 56, 107, 22, 214, 255, 0, 145, 127, 114, 0, 255, 109, 119, 28, 104, 94, 165, 247, 145, 45, 35, 2, 40, 237, 135, 91, 168, 0, 63, 0, 45, 79, 22, 53, 79, 68, 239, 142, 255, 255, 168, 66, 51, 221, 58, 10, 96, 51, 201, 216, 170, 186, 28, 175, 175, 229, 45, 158, 0, 119, 201, 96, 255, 186, 15, 124, 229, 204, 239, 193, 86, 130, 229, 173, 122, 160, 252, 247, 249, 160, 255, 255, 234, 244, 17, 214, 216, 102, 102, 127, 204, 104, 255, 132, 249, 211, 237, 229, 155, 198, 168, 155, 181, 114, 160, 153, 178, 206, 135, 196, 140, 142, 255, 163, 255, 255, 204, 193, 89, 239, 153, 160, 73, 226, 33, 0, 84, 30, 63, 0, 239, 244, 63, 237, 79, 216, 153, 130, 193, 0, 33, 0, 48, 175, 94, 153, 193, 183, 221, 0, 5, 63, 229, 79, 53, 160, 84, 102, 112, 51, 63, 178, 216, 104, 219, 249, 76, 209, 68, 96, 20, 183, 221, 188, 124, 124, 0, 25, 73, 249, 196, 181, 255, 255, 226, 234, 112, 168, 12, 153, 186, 122, 68, 96, 73, 219, 119, 173, 127, 216, 221, 0, 86, 163, 63, 255, 79, 130, 221, 142, 255, 119, 119, 127, 48, 181, 244, 104, 48, 124, 127, 132, 127, 142, 51, 186, 56, 132, 127, 153, 165, 158, 68, 112, 188, 79, 66, 73, 109, 33, 0, 102, 58, 40, 211, 237, 117, 63, 196, 173, 137, 204, 191, 232, 249, 209, 132, 216, 160, 112, 221, 191, 234, 153, 237, 112, 22, 239, 198, 198, 104, 206, 221, 153, 178, 165, 158, 104, 153, 252, 0, 104, 229, 204, 204, 216, 221, 226, 224, 216, 104, 226, 204, 56, 165, 17, 142, 127, 28, 51, 40, 89, 132, 0, 15, 53, 10, 221, 119, 191, 221, 153, 170, 142, 196, 226, 68, 153, 96, 89, 224, 142, 28, 48, 0, 135, 117, 0, 0, 33, 53, 119, 79, 63, 71, 107, 204, 137, 53, 173, 10, 94, 79, 68, 102, 51, 38, 79, 0, 0, 2, 28, 38, 40, 0, 84, 20, 79, 45, 35, 117, 63, 165, 0, 145, 107, 102, 48, 71, 204, 130, 137, 0, 66, 71, 109, 38, 102, 153, 91, 79, 0, 0, 142, 56, 35, 104, 43, 81, 216, 68, 0, 17, 17, 0, 102, 142, 28, 68, 22, 63, 43, 86, 68, 102, 196, 40, 140, 145, 178, 112, 214, 163, 112, 0, 124, 81, 102, 160, 35, 12, 145, 216, 255, 104, 255, 20, 244, 186, 66, 119, 206, 158, 211, 135, 15, 112, 45, 191, 170, 173, 193, 63, 114, 206, 112, 89, 127, 51, 63, 130, 61, 12, 249, 198, 117, 17, 40, 117, 0, 25, 96, 255, 191, 252, 255, 122, 130, 51, 216, 0, 102, 216, 89, 204, 214, 107, 181, 76, 132, 204, 112, 61, 132, 239, 130, 193, 127, 117, 229, 130, 51, 86, 114, 191, 112, 137, 186, 140, 214, 229, 99, 107, 198, 12, 127, 117, 114, 124, 135, 170, 114, 214, 255, 214, 214, 79, 48, 122, 153, 73, 2, 51, 0, 119, 104, 178, 209, 10, 102, 112, 81, 221, 145, 135, 255, 68, 17, 33, 173, 79, 0, 204, 127, 0, 229, 142, 178, 7, 178, 66, 56, 33, 0, 0, 0, 130, 73, 84, 130, 38, 28, 28, 0, 204, 160, 0, 66, 244, 84, 221, 255, 244, 173, 112, 66, 107, 84, 45, 48, 160, 153, 135, 76, 40, 255, 204, 211, 239, 255, 204, 173, 239, 20, 22, 165 };
        static int[] namesB = new int[] { 232, 229, 191, 168, 142, 193, 56, 255, 53, 15, 204, 79, 86, 0, 0, 61, 204, 56, 244, 117, 30, 130, 45, 214, 0, 0, 175, 255, 211, 33, 73, 107, 181, 107, 102, 40, 0, 127, 2, 255, 255, 239, 242, 193, 249, 175, 204, 181, 53, 137, 2, 130, 122, 229, 112, 219, 147, 66, 196, 30, 22, 12, 17, 94, 81, 0, 2, 40, 53, 204, 99, 232, 237, 191, 255, 255, 175, 188, 153, 255, 209, 204, 186, 127, 226, 247, 239, 130, 58, 181, 201, 0, 79, 96, 255, 66, 84, 214, 0, 226, 71, 127, 221, 232, 255, 163, 127, 38, 51, 0, 0, 40, 35, 61, 204, 255, 130, 7, 33, 135, 0, 81, 35, 163, 99, 114, 160, 175, 61, 45, 33, 0, 91, 33, 43, 173, 107, 204, 107, 0, 0, 122, 255, 33, 58, 153, 22, 63, 66, 56, 201, 28, 221, 33, 63, 119, 73, 206, 175, 165, 124, 255, 209, 99, 130, 165, 191, 193, 165, 48, 89, 206, 79, 43, 170, 0, 0, 99, 196, 53, 160, 109, 30, 135, 0, 30, 0, 122, 51, 30, 10, 30, 51, 232, 170, 30, 61, 56, 255, 119, 99, 170, 51, 102, 104, 81, 102, 0, 79, 119, 63, 68, 91, 28, 237, 219, 232, 216, 209, 61, 51, 255, 234, 124, 48, 48, 140, 153, 33, 84, 0, 124, 96, 68, 140, 119, 10, 168, 33, 107, 33, 107, 51, 150, 140, 102, 35, 45, 0, 204, 204, 61, 214, 33, 127, 153, 86, 0, 122, 25, 142, 20, 214, 140, 79, 68, 81, 17, 51, 91, 209, 168, 211, 12, 61, 84, 84, 61, 56, 43, 135, 165, 71, 63, 193, 73, 76, 186, 204, 211, 58, 163, 147, 91, 51, 255, 107, 96, 76, 89, 188, 107, 175, 255, 104, 84, 255, 104, 102, 22, 155, 201, 94, 79, 127, 81, 214, 165, 255, 63, 255, 0, 255, 255, 0, 255, 255, 255, 51, 119, 61, 147, 81, 91, 163, 168, 107, 22, 137, 132, 160, 112, 84, 130, 66, 0, 30, 33, 40, 33, 170, 35, 142, 130, 239, 0, 147, 0, 79, 33, 33, 91, 76, 186, 142, 56, 211, 71, 137, 255, 68, 255, 255, 193, 255, 117, 0, 102, 219, 15, 255, 28, 0, 181, 249, 102, 56, 0, 79, 20, 0, 0, 33, 160, 168, 127, 127, 191, 68, 170, 0, 119, 0, 119, 107, 79, 51, 45, 135, 127, 84, 206, 249, 107, 0, 22, 0, 0, 255, 160, 239, 175, 107, 206, 181, 58, 209, 94, 119, 107, 147, 61, 56, 91, 7, 91, 86, 255, 107, 130, 165, 0, 12, 43, 206, 107, 237, 0, 255, 239, 107, 71, 86, 124, 61, 94, 79, 51, 22, 86, 135, 22, 5, 158, 145, 140, 22, 196, 12, 48, 221, 155, 102, 158, 33, 219, 249, 255, 244, 209, 234, 237, 249, 209, 181, 226, 0, 0, 204, 28, 0, 0, 191, 79, 15, 175, 229, 28, 112, 127, 234, 145, 255, 239, 209, 211, 142, 140, 204, 173, 214, 216, 193, 81, 122, 153, 170, 249, 153, 221, 109, 170, 224, 198, 0, 0, 51, 10, 5, 229, 107, 219, 79, 204, 33, 255, 163, 122, 124, 142, 209, 255, 0, 91, 219, 81, 170, 66, 102, 137, 71, 0, 96, 255, 109, 170, 249, 58, 170, 204, 43, 53, 170, 150, 45, 221, 211, 165, 219, 132, 104, 112, 234, 237, 135, 153, 71, 204, 58, 58, 132, 119, 124, 181, 140, 56, 124, 112, 84, 135, 12, 137, 249, 153, 224, 214, 22, 193, 0, 91, 142, 140, 58, 48, 140, 89, 114, 198, 0, 94, 173, 127, 234, 66, 99, 20, 198, 127, 237, 51, 219, 191, 33, 0, 45, 58, 229, 119, 71, 53, 130, 130, 0, 35, 30, 114, 56, 165, 0, 2, 0, 0, 0, 214, 204, 20, 33, 76, 73, 71, 0, 0, 226, 96, 229, 237, 84, 53, 226, 175, 102, 239, 137, 170, 153, 255, 229, 221, 221, 147, 209, 186, 188, 124, 237, 147, 73, 214, 119, 206, 84, 196, 119, 193, 71, 163, 181, 96, 201, 150, 127, 119, 181, 163, 153, 186, 173, 48, 198, 191, 163, 0, 255, 186, 147, 122, 89, 191, 28, 51, 163, 0, 63, 255, 137, 35, 79, 229, 112, 204, 244, 102, 206, 165, 114, 226, 132, 204, 142, 53, 229, 0, 28, 84, 255, 153, 22, 127, 196, 239, 155, 181, 216, 76, 79, 147, 214, 168, 94, 96, 91, 109, 153, 107, 68, 204, 107, 132, 0, 61, 51, 35, 17, 40, 17, 73, 132, 81, 127, 135, 150, 63, 255, 63, 209, 206, 209, 96, 51, 204, 140, 150, 127, 158, 68, 119, 53, 204, 168, 91, 81, 10, 0, 142, 168, 102, 224, 145, 168, 94, 117, 86, 94, 30, 40, 40, 150, 7, 28, 76, 12, 66, 63, 17, 0, 48, 122, 104, 163, 127, 22, 63, 96, 22, 10, 40, 186, 165, 53, 0, 53, 175, 0, 112, 147, 140, 20, 237, 0, 20, 91, 242, 96, 0, 147, 191, 255, 22, 191, 170, 173, 193, 10, 114, 234, 175, 204, 142, 153, 135, 117, 63, 7, 239, 117, 117, 22, 81, 51, 71, 76, 0, 255, 252, 0, 127, 183, 181, 204, 94, 0, 107, 112, 140, 51, 165, 168, 140, 0, 0, 0, 122, 51, 137, 191, 119, 193, 127, 135, 178, 127, 117, 0, 91, 216, 160, 170, 181, 61, 209, 0, 71, 191, 124, 53, 127, 94, 206, 193, 140, 135, 130, 198, 239, 181, 165, 56, 48, 91, 153, 107, 61, 170, 76, 193, 147, 0, 112, 142, 244, 255, 71, 201, 229, 38, 102, 33, 17, 40, 33, 153, 0, 0, 0, 63, 170, 168, 89, 20, 173, 51, 30, 239, 255, 255, 175, 237, 178, 147, 109, 35, 53, 130, 255, 255, 137, 255, 66, 249, 81, 178, 255, 244, 209, 163, 163, 132, 2, 56, 71, 219, 107, 119, 145, 63, 0, 0, 0, 0, 51, 51, 66, 0, 168, 7, 142 };
        public static string RGBToNamedColor(int r, int g, int b, bool valuesOnSecondLine = false)
        {
            var curPointerColor = RGBToLab(r, g, b);
            double minDistance = double.MaxValue;
            int minIndex = 0;

            for (int i = 0; i < names.Length; i++)
            {
                var curNamedColorRgb = System.Drawing.Color.FromArgb(namesR[i], namesG[i], namesB[i]);
                var curNamedColor = RGBToLab(curNamedColorRgb.R, curNamedColorRgb.G, curNamedColorRgb.B);

                var curDistance = GetDistance(
                   curPointerColor[0], curNamedColor[0],
                   curPointerColor[1], curNamedColor[1],
                   curPointerColor[2], curNamedColor[2]
                );

                if (minDistance > curDistance)
                {
                    minDistance = curDistance;
                    minIndex = i;
                }
            }

            return names[minIndex] + (valuesOnSecondLine ? System.Environment.NewLine : " ") +  "(" + namesR[minIndex] + "," + namesG[minIndex] + "," + namesB[minIndex] + ")";
        }

        static double GetDistance(double l1, double l2, double a1, double a2, double b1, double b2)
        {
            return Math.Sqrt(
               Math.Pow(l1 - l2, 2) +
               Math.Pow(a1 - a2, 2) +
               Math.Pow(b1 - b2, 2)
            );
        }
    }
}
