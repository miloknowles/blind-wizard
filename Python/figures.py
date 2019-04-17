from calculator import *
from matplotlib import pyplot as plt
import numpy as np

def ExamplePrior():
  """
  Prior over water, fire, air, earth for ocean region.
  """
  x = np.arange(4)
  p_type_given_ocean = [0.4, 0.1, 0.2, 0.3]
  plt.bar(x, p_type_given_ocean, color=('DarkBlue', 'Red', 'LightBlue', 'Green'))
  plt.xticks(x, ('Water', 'Fire', 'Air', 'Earth'))
  plt.yticks(np.arange(0, 1.1, 0.1))
  plt.title('Distribution of Enemy Types in Ocean')
  plt.show()

def ExampleGivenFurryAndOcean():
  x = np.arange(4)
  posterior = ComputePosterior('Ocean', 'Furry')
  plt.bar(x, posterior, color=('DarkBlue', 'Red', 'LightBlue', 'Green'))
  plt.xticks(x, ('Water', 'Fire', 'Air', 'Earth'))
  plt.yticks(np.arange(0, 1.1, 0.1))
  plt.title('Distribution of Enemy Types: Furry and in Ocean')
  plt.show()

def ExampleGivenMove():
  MOVES = [ Move('Air', Punch, False) ]
  ENEMY_MOVES = [ Move('Fire', Punch, True) ]
  x = np.arange(4)
  posterior = ComputePosterior('Ocean', 'Furry', MOVES, ENEMY_MOVES)
  plt.bar(x, posterior, color=('DarkBlue', 'Red', 'LightBlue', 'Green'))
  plt.xticks(x, ('Water', 'Fire', 'Air', 'Earth'))
  plt.yticks(np.arange(0, 1.1, 0.1))
  plt.title('Distribution of Enemy Types: Enemy Attack Hits')
  plt.show()

# ExamplePrior()
# ExampleGivenFurryAndOcean()
ExampleGivenMove()
