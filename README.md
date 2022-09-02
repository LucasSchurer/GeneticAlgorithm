# Genetic Algorithm in a Top Down Shooter

The goal of this project is to test a procedural enemy generation approach using genetic algorithm. It is currently being developed and in its early stages. However, the genetic algorithm is already implemented on a basic environment.

# Genes

Currently, there's only three genes for each enemy: 
1. Color;
2. Weapon (rifle or sniper);
3. Behaviour (aggressive, stationary or cautious).

# Examples

For both examples, the fitness function only considers the damage a enemy has dealt to the player.

Example 1:
- Mutation Rate: 0.35;
- Population Size: 4;
- Rifle RoF: 0.2;
- Sniper RoF: 1;
- Rifle Damage: 1;
- Sniper Damage: 10  

https://user-images.githubusercontent.com/39245000/188213222-08f57862-8293-4ba8-889c-7106d456025a.mp4

Example 2:
- Mutation Rate: 0.35;
- Population Size: 4;
- Rifle RoF: 0.2;
- Sniper RoF: 0.4;
- Rifle Damage: 0;
- Sniper Damage: 10  

https://user-images.githubusercontent.com/39245000/188213808-d9ab8f62-ca9e-4d9f-8b4b-dc567c174a48.mp4

Due to the low population size and high mutation rate, you can see that almost half of the second example population go back to using rifles from the third generation, but from the first to the second generation, all of the entities were using sniper.
