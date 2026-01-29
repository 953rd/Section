using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static string[,,,] section;
    static Random random = new Random();
    
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.Title = "Поиск в 4D массиве - Компактная таблица";
        
        InitializeArray();
        PrintArray();
        SearchNearestPositions();
        
        Console.WriteLine("\nНажмите любую клавишу для выхода...");
        Console.ReadKey();
    }
    
    static string NormalizeValue(string value)
    {
        if (string.IsNullOrEmpty(value))
            return value;
            
        var replacements = new Dictionary<char, char>
        {
            {'T', 'Т'}, {'t', 'т'}, {'Y', 'У'}, {'y', 'у'}
        };
        
        char[] chars = value.ToCharArray();
        for (int i = 0; i < chars.Length; i++)
        {
            if (replacements.ContainsKey(chars[i]))
                chars[i] = replacements[chars[i]];
        }
        
        return new string(chars);
    }
    
    static void InitializeArray()
    {
        Console.WriteLine("Инициализация массива...");
        section = new string[4, 4, 20, 1];
        
        string[] possibleValues = {
            "1", "1Т", "2", "2Т", "3", "3Т", 
            "2у", "2Ту", "3у", "3Ту", 
            "11", "11Т", "12", "12Т", "13", "13Т", "14", "14Т", 
            "15/75", "15/85", "15/85rgb", "15Т", 
            "17/75", "17/85", "17/85rgb", "17/85rgbТ", 
            "19", "19Т"
        };
        
        for (int i = 0; i < section.GetLength(0); i++)
        {
            for (int j = 0; j < section.GetLength(1); j++)
            {
                for (int k = 0; k < section.GetLength(2); k++)
                {
                    int randomIndex = random.Next(possibleValues.Length);
                    section[i, j, k, 0] = possibleValues[randomIndex];
                }
            }
        }
        Console.WriteLine("Массив заполнен случайными значениями!\n");
    }
    
static void PrintArray()
{
    const int CELL_WIDTH = 10; // Ширина ячейки для значений
    const int ROW_HEADER_WIDTH = 6; // Ширина заголовка строки
    
    Console.WriteLine("ТАБЛИЦА ЗНАЧЕНИЙ (4 блока × 4 строки × 20 колонок)");
    Console.WriteLine(new string('═', ROW_HEADER_WIDTH + 20 * CELL_WIDTH + 21));
    
    for (int block = 0; block < 4; block++)
    {
        Console.WriteLine($"\nБЛОК #{block + 1}");
        Console.WriteLine(new string('─', ROW_HEADER_WIDTH + 20 * CELL_WIDTH + 21));
        
        // Верхняя граница
        Console.Write("┌" + new string('─', ROW_HEADER_WIDTH) + "┬");
        for (int col = 0; col < 20; col++)
        {
            Console.Write(new string('─', CELL_WIDTH));
            if (col < 19) Console.Write("┬");
        }
        Console.WriteLine("┐");
        
        // Заголовки колонок
        Console.Write($"│{" ",ROW_HEADER_WIDTH}│");
        for (int col = 1; col <= 20; col++)
        {
            Console.Write($"{col.ToString().PadLeft((CELL_WIDTH + 1) / 2).PadRight(CELL_WIDTH)}│");
        }
        Console.WriteLine();
        
        // Разделитель заголовков
        Console.Write("├" + new string('─', ROW_HEADER_WIDTH) + "┼");
        for (int col = 0; col < 20; col++)
        {
            Console.Write(new string('─', CELL_WIDTH));
            if (col < 19) Console.Write("┼");
        }
        Console.WriteLine("┤");
        
        for (int row = 0; row < 4; row++)
        {
            // Номер строки
            Console.Write($"│{row + 1,ROW_HEADER_WIDTH}│");
            
            // Значения в ячейках
            for (int col = 0; col < 20; col++)
            {
                string value = section[block, row, col, 0];
                // Выравнивание по центру
                int padding = CELL_WIDTH - value.Length;
                int leftPadding = padding / 2;
                int rightPadding = padding - leftPadding;
                
                Console.Write($"{new string(' ', leftPadding)}{value}{new string(' ', rightPadding)}│");
            }
            Console.WriteLine();
            
            // Разделитель строк (кроме последней)
            if (row < 3)
            {
                Console.Write("├" + new string('─', ROW_HEADER_WIDTH) + "┼");
                for (int col = 0; col < 20; col++)
                {
                    Console.Write(new string('─', CELL_WIDTH));
                    if (col < 19) Console.Write("┼");
                }
                Console.WriteLine("┤");
            }
        }
        
        // Нижняя граница
        Console.Write("└" + new string('─', ROW_HEADER_WIDTH) + "┴");
        for (int col = 0; col < 20; col++)
        {
            Console.Write(new string('─', CELL_WIDTH));
            if (col < 19) Console.Write("┴");
        }
        Console.WriteLine("┘");
        
        // Разделитель блоков (кроме последнего)
        if (block < 3)
        {
            Console.WriteLine("\n" + new string('·', ROW_HEADER_WIDTH + 20 * CELL_WIDTH + 21));
        }
    }
}
    
    static void SearchNearestPositions()
    {
        while (true)
        {
            Console.WriteLine("\n╔══════════════════════════════════════════════════════╗");
            Console.WriteLine("║  ПОИСК БЛИЖАЙШИХ ПОЗИЦИЙ                             ║");
            Console.WriteLine("╠══════════════════════════════════════════════════════╣");
            Console.WriteLine("║ Введите значения для поиска через запятую:           ║");
            Console.WriteLine("║ Пример: 1, 15/75, 17Т, 19Т                           ║");
            Console.WriteLine("║ Или введите 'выход' для завершения                   ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════╝");
            
            Console.Write("\n> Ваш запрос: ");
            string input = Console.ReadLine();
            
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Пожалуйста, введите значения для поиска!");
                continue;
            }
                
            if (input.ToLower() == "выход" || input.ToLower() == "exit")
            {
                Console.WriteLine("Завершение работы программы...");
                break;
            }
            
            // Обработка ввода с нормализацией
            string[] searchValues = input.Split(',')
                .Select(x => NormalizeValue(x.Trim()))
                .Where(x => !string.IsNullOrEmpty(x))
                .ToArray();
            
            if (searchValues.Length == 0)
            {
                Console.WriteLine("Ошибка: не введены значения для поиска!");
                continue;
            }
            
            // Ищем все позиции для каждого значения
            var positions = new Dictionary<string, List<(int block, int row, int col, int depth)>>();
            
            // Заполняем словарь позициями
            for (int b = 0; b < 4; b++)
            {
                for (int r = 0; r < 4; r++)
                {
                    for (int c = 0; c < 20; c++)
                    {
                        string value = section[b, r, c, 0];
                        if (searchValues.Contains(value))
                        {
                            if (!positions.ContainsKey(value))
                                positions[value] = new List<(int block, int row, int col, int depth)>();
                            
                            positions[value].Add((b + 1, r + 1, c + 1, 1));
                        }
                    }
                }
            }
            
            // Выводим результаты
            Console.WriteLine("\n" + new string('═', 60));
            Console.WriteLine("РЕЗУЛЬТАТЫ ПОИСКА:");
            Console.WriteLine(new string('═', 60));
            
            bool foundAny = false;
            
            foreach (string searchValue in searchValues)
            {
                Console.Write($"\nЗначение '{searchValue}': ");
                
                if (positions.ContainsKey(searchValue))
                {
                    foundAny = true;
                    var posList = positions[searchValue];
                    Console.WriteLine($"НАЙДЕНО {posList.Count} позиций");
                    
                    // Группируем по блокам для удобного вывода
                    var groupedByBlock = posList.GroupBy(p => p.block)
                                                .OrderBy(g => g.Key);
                    
                    foreach (var group in groupedByBlock)
                    {
                        Console.WriteLine($"  Ряд {group.Key}:");
                        
                        foreach (var pos in group.OrderBy(p => p.row).ThenBy(p => p.col))
                        {
                            Console.WriteLine($"    Ярус {pos.row}, Ячейка {pos.col}");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("НЕ НАЙДЕНО");
                    FindSimilarValues(searchValue);
                }
            }
            
            ShowStatistics(positions, searchValues);
            VisualizeSearch(positions, searchValues);
            
            Console.WriteLine("\n" + new string('═', 60));
        }
    }
    
    static void FindSimilarValues(string searchValue)
    {
        List<string> similar = new List<string>();
        
        for (int b = 0; b < 4; b++)
        {
            for (int r = 0; r < 4; r++)
            {
                for (int c = 0; c < 20; c++)
                {
                    string value = section[b, r, c, 0];
                    
                    string searchBase = searchValue.Replace("Т", "").Replace("т", "").Replace("у", "").Replace("У", "");
                    string valueBase = value.Replace("Т", "").Replace("т", "").Replace("у", "").Replace("У", "");
                    
                    if (valueBase == searchBase || 
                        value.Contains(searchValue) || 
                        searchValue.Contains(value))
                    {
                        if (!similar.Contains(value))
                            similar.Add(value);
                    }
                }
            }
        }
        
        if (similar.Count > 0)
        {
            Console.WriteLine($"    Возможно вы искали: {string.Join(", ", similar.Distinct().Take(5))}");
        }
    }
    
    static void ShowStatistics(Dictionary<string, List<(int block, int row, int col, int depth)>> positions,
                              string[] searchValues)
    {
        Console.WriteLine("\n" + new string('═', 60));
        Console.WriteLine("СТАТИСТИКА:");
        Console.WriteLine(new string('═', 60));
        
        int totalFound = 0;
        int totalNotFound = 0;
        
        foreach (string value in searchValues)
        {
            if (positions.ContainsKey(value))
            {
                int count = positions[value].Count;
                totalFound += count;
                Console.WriteLine($"  {value,-12}: найдено {count,3} раз");
            }
            else
            {
                totalNotFound++;
                Console.WriteLine($"  {value,-12}: не найдено");
            }
        }
        
        Console.WriteLine($"\n  Всего найдено: {totalFound} позиций");
        Console.WriteLine($"  Не найдено: {totalNotFound} значений");
        
    }
    
    static void VisualizeSearch(Dictionary<string, List<(int block, int row, int col, int depth)>> positions,
                               string[] searchValues)
    {
        Console.WriteLine("\n" + new string('═', 80));
        Console.WriteLine("ВИЗУАЛИЗАЦИЯ РАСПОЛОЖЕНИЯ:");
        Console.WriteLine(new string('═', 80));
        
        Dictionary<string, ConsoleColor> colorMap = new Dictionary<string, ConsoleColor>();
        ConsoleColor[] colors = {
            ConsoleColor.Red, ConsoleColor.Green, ConsoleColor.Yellow,
            ConsoleColor.Blue, ConsoleColor.Magenta, ConsoleColor.Cyan,
            ConsoleColor.DarkRed, ConsoleColor.DarkGreen, ConsoleColor.DarkYellow
        };
        
        for (int i = 0; i < searchValues.Length && i < colors.Length; i++)
        {
            colorMap[searchValues[i]] = colors[i];
        }
        
        for (int block = 1; block <= 4; block++)
        {
            Console.WriteLine($"\nРяд #{block}:");
            Console.Write("     ");
            Console.WriteLine();
            
            Console.WriteLine("    " + new string('─', 20));
            
            for (int row = 1; row <= 4; row++)
            {
                Console.Write($" {row} │ ");
                
                for (int col = 1; col <= 20; col++)
                {
                    string foundValue = null;
                    foreach (var kvp in positions)
                    {
                        if (kvp.Value.Any(pos => pos.block == block && 
                                                pos.row == row && 
                                                pos.col == col))
                        {
                            foundValue = kvp.Key;
                            break;
                        }
                    }
                    
                    if (foundValue != null && colorMap.ContainsKey(foundValue))
                    {
                        Console.ForegroundColor = colorMap[foundValue];
                        Console.Write("█");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.Write("·");
                    }
                }
                Console.WriteLine();
            }
        }
        
        Console.WriteLine("\nЛегенда:");
        foreach (var kvp in colorMap)
        {
            Console.ForegroundColor = kvp.Value;
            Console.Write("█ ");
            Console.ResetColor();
            Console.WriteLine($"- {kvp.Key}");
        }
        Console.ResetColor();
    }
}