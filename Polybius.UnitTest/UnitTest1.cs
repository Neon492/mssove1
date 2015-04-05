using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Polybius.UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        private readonly string[] keys =
        {
            "А все-таки она вертится!",
            "Каштаны из огня таскать",
            "Казанская сирота",
            "Лучше меньше, да лучше",
            "Ящик Пандоры",
            "Табула раза (tabula rasa)"
        };

        private readonly string[] texts =
        {
            "Так в 1923 году В. И. Ленин озаглавил свою замечательную статью о мерах, которые необходимо было принять для укрепления и улучшения советского государственного аппарата. Слова эти оказались столь многозначительными и вескими, были так удачно найдены, что скоро из простого заглавия превратились в настоящее крылатое слово со значением: качество может быть важнее количества.",
            "В огромных старорусских семьях было принято, чтобы их члены чередовались понедельно на домашних работах: эту неделю Федор колет дрова, Иван таскает воду, следующую - наоборот. Так же поочередно мололи муку на домашнем жернове: очень нелегкая работа. Болтунам же, отлынивавшим от всякого труда, говорили с насмешкой: \"Мели, Емеля, твоя неделя!\" - играя на том, что выражение \"языком молоть\" комически сопоставлялось тут именно с представлением о самой тяжелой работе на жернове.",
            "Выражение это, так сказать, царского происхождения. Автором его был царь Иван IV, прозванный в народе Грозным за массовые казни и убийства. Для усиления своей самодержавной власти, что невозможно было без ослабления князей, бояр и духовенства, Иван Грозный ввел опричнину, наводившую ужас на все государство Российское.Не мог примириться с разгулом опричников и митрополит Московский Филипп.",
            "Что это за выражение? При чем тут решето? Оказывается, это сокращение старинной русской поговорки, которая гласила: \"чудеса в решете - дыр много, а выскочить некуда\". До нас дошла только первая часть фразы, которая как бы вобрала в себя смысл всего высказывания.",
            "В греческой и римской мифологии Цербер - чудовищный трехглавый пес со змеиным хвостом, охранявший вход в подземное царство Аида, который был укрощен Гераклом. Отсюда слово \"цербер\" употребляется в значении: злой, свирепый надсмотрщик.",
            "Когда великий титан Прометей похитил с Олимпа и передал людям огонь богов, отец богов Зевс страшно покарал смельчака, но было поздно. Обладая божественным пламенем, люди перестали подчиняться небожителям, научились разным наукам, вышли из своего жалкого состояния. Еще немного - и они завоевали бы себе полное счастье...",
            "По древнегреческому мифу, Юпитеру (греч. Зевс) приглянулась дочь финикийского царя Европа. Юпитер превратился в быка и похитил ее. Quod licet Jovi, non licet bovi - пословица говорит о нескромной или безосновательной претензии."
        };
        Random rnd = new Random((int) DateTime.Now.Ticks);

        [TestMethod]
        public void TestMethod1()
        {
            try
            {
                Console.WriteLine(@"Квадрат Полибия");
                Console.WriteLine();
                PolybiusCryptography cryptography = new PolybiusCryptography();
                for (int i = 0; i < 20; i++)
                {
                    string key = keys[i%keys.Length];
                    string text = texts[i%texts.Length];
                    Console.WriteLine(@"Тест #:               " + i);
                    Console.WriteLine(@"Ключ:                 " + key);
                    Console.WriteLine(@"Исходный текст:       " + text);
                    cryptography.SetKey(key);
                    string cipher = cryptography.EncryptNext(text);
                    Console.WriteLine(@"Шифрованный текст:    " + cipher);
                    cryptography.Restart();
                    string plain = cryptography.DecryptNext(cipher);
                    Console.WriteLine(@"Расшифрованный текст: " + plain);
                    Assert.IsTrue(string.Compare(text, cipher) != 0);
                    Assert.IsTrue(string.Compare(text, plain) == 0);
                    Console.WriteLine();
                }
            }
            catch (Exception)
            {
                Assert.IsTrue(false);
            }
        }

        [TestMethod]
        public void TestMethod2()
        {
            try
            {
                Console.WriteLine(@"Квадрат Полибия (модификация)");
                Console.WriteLine();
                PolybiusCryptography cryptography = new PolybiusCryptography();
                for (int i = 0; i < 20; i++)
                {
                    string key = keys[i % keys.Length];
                    int additionalKey = 1+rnd.Next()%100;
                    string text = texts[i % texts.Length];
                    Console.WriteLine(@"Тест #:               " + i);
                    Console.WriteLine(@"Ключ:                 " + key);
                    Console.WriteLine(@"Дополнительный ключ:  " + additionalKey);
                    Console.WriteLine(@"Исходный текст:       " + text);
                    cryptography.SetKey(key);
                    cryptography.SetAdditionalKey(additionalKey);
                    string cipher = cryptography.EncryptNext(text);
                    Console.WriteLine(@"Шифрованный текст:    " + cipher);
                    cryptography.Restart();
                    string plain = cryptography.DecryptNext(cipher);
                    Console.WriteLine(@"Расшифрованный текст: " + plain);
                    Assert.IsTrue(string.Compare(text, cipher) != 0);
                    Assert.IsTrue(string.Compare(text, plain) == 0);
                    Console.WriteLine();
                }
            }
            catch (Exception)
            {
                Assert.IsTrue(false);
            }
        }
    }
}